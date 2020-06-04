using ReactiveUI;
using SkyEditor.RomEditor.Avalonia.ViewModels.MenuItems;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SkyEditor.RomEditor.Avalonia.ViewModels
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
