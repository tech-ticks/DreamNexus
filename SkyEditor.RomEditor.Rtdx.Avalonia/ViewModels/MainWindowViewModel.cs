using ReactiveUI;
using SkyEditor.RomEditor.Avalonia.ViewModels.MenuItems;
using SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx.Tutorial;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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
            RunAutomationScriptMenuItem = new ApplyModMenuItem(this);

            // Start out with a tutorial for the most common functions
            var intro = new IntroViewModel(this);
            OpenFiles.Add(intro);
            CurrentFile = intro;
        }

        public OpenDirectoryMenuItem OpenDirectoryMenuItem { get; }
        public SaveDirectoryAsMenuItem SaveDirectoryAsMenuItem { get; }
        public SaveMenuItem SaveMenuItem { get; }
        public CreateAutomationScriptMenuItem CreateAutomationScriptMenuItem { get; }
        public ApplyModMenuItem RunAutomationScriptMenuItem { get; }

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


        public void OpenFile(OpenFileViewModel viewModel, bool closeOtherFiles)
        {
            if (OpenFiles.Any())
            {
                // Let's keep the UI simple for now
                // In the future we should only close the other files if closeOtherFiles is true
                CloseAllFiles();
            }

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
