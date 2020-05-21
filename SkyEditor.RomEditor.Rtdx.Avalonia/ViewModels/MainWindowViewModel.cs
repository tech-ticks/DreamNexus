using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.Rtdx;
using SkyEditor.RomEditor.Rtdx.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            OpenFileCommand = ReactiveCommand.Create(OpenFile);
            OpenFiles = new ObservableCollection<OpenFileViewModel>();
        }

        public ReactiveCommand<Unit, Task> OpenFileCommand { get; }

        public ObservableCollection<OpenFileViewModel> OpenFiles { get; set; }

        public OpenFileViewModel? CurrentFile
        {
            get => _openFile;
            set => this.RaiseAndSetIfChanged(ref _openFile, value);
        }
        private OpenFileViewModel? _openFile;

        private async Task OpenFile()
        {
            var dialog = new OpenFolderDialog();
            var path = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            if (!string.IsNullOrEmpty(path))
            {
                if (OpenFiles.Any())
                {
                    // Let's keep the UI simple for now
                    CloseFiles();
                }

                var rom = new RtdxRom(path, PhysicalFileSystem.Instance);
                OpenFile(new RtdxRomViewModel(rom));
            }
        }

        private void OpenFile(OpenFileViewModel viewModel)
        {
            OpenFiles.Add(viewModel);
            CurrentFile = viewModel;
        }

        private void CloseFiles()
        {
            CurrentFile = null;
            OpenFiles.Clear();
        }
    }
}
