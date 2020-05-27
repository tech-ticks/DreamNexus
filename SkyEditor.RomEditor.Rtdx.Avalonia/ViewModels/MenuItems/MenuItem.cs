using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.MenuItems
{
    /// <summary>
    /// Base class for a logical menu item
    /// </summary>
    public abstract class MenuItem
    {
        public MenuItem()
        {
            CanExecuteSubject = new Subject<bool>();
            Command = ReactiveCommand.Create(Execute, CanExecuteSubject);
            CanExecute = true;
        }

        public ReactiveCommand<Unit, Task> Command { get; }

        private Subject<bool> CanExecuteSubject { get; set; }

        protected bool CanExecute
        {
            set
            {
                CanExecuteSubject.OnNext(value);
            }
        }

        protected abstract Task Execute();
    }

    /// <summary>
    /// Base class for a logical menu item that operates on a particular type of file view model
    /// </summary>
    /// <typeparam name="TViewModel">Type of the target view model</typeparam>
    public abstract class MenuItem<TViewModel> : MenuItem, IDisposable where TViewModel : OpenFileViewModel
    {
        public MenuItem(MainWindowViewModel mainWindow)
        {
            this.mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            this.mainWindow.PropertyChanged += MainWindow_OnPropertyChanged;
            this.CanExecute = this.SupportsViewModel(mainWindow.CurrentFile);
        }

        protected readonly MainWindowViewModel mainWindow;

        protected override async Task Execute()
        {
            if (mainWindow.CurrentFile is TViewModel viewModel)
            {
                await this.Execute(viewModel);
            }
        }

        protected abstract Task Execute(TViewModel viewModel);

        /// <summary>
        /// Determines whether the menu item supports the given view model
        /// </summary>
        protected virtual bool SupportsViewModel(TViewModel viewModel) => true;

        private bool SupportsViewModel(OpenFileViewModel? viewModel) => viewModel is TViewModel currentFileViewModel && SupportsViewModel(currentFileViewModel);

        private void MainWindow_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is MainWindowViewModel mainWindowViewModel && e.PropertyName == (nameof(MainWindowViewModel.CurrentFile)))
            {
                this.CanExecute = SupportsViewModel(mainWindowViewModel.CurrentFile);
            }
        }

        public void Dispose()
        {
            this.mainWindow.PropertyChanged -= MainWindow_OnPropertyChanged;
        }
    }
}
