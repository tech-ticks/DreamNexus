using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.MenuItems;
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
            OpenFiles = new ObservableCollection<OpenFileViewModel>();

            OpenDirectoryMenuItem = new OpenDirectoryMenuItem(this);
            SaveDirectoryAsMenuItem = new SaveDirectoryAsMenuItem(this);
            SaveMenuItem = new SaveMenuItem(this);
            CreateAutomationScriptMenuItem = new CreateAutomationScriptMenuItem(this);
            RunAutomationScriptMenuItem = new RunAutomationScriptMenuItem(this);
        }

        public OpenDirectoryMenuItem OpenDirectoryMenuItem { get; }
        public SaveDirectoryAsMenuItem SaveDirectoryAsMenuItem { get; }
        public SaveMenuItem SaveMenuItem { get; }
        public CreateAutomationScriptMenuItem CreateAutomationScriptMenuItem { get; }
        public RunAutomationScriptMenuItem RunAutomationScriptMenuItem { get; }

        public ObservableCollection<OpenFileViewModel> OpenFiles { get; set; }

        public OpenFileViewModel? CurrentFile
        {
            get => _openFile;
            set
            {
                this.RaiseAndSetIfChanged(ref _openFile, value);
            }
        }
        private OpenFileViewModel? _openFile;


        public void OpenFile(OpenFileViewModel viewModel)
        {
            OpenFiles.Add(viewModel);
            CurrentFile = viewModel;
        }

        public void CloseAllFiles()
        {
            CurrentFile = null;
            OpenFiles.Clear();
        }
    }
}
