using System;
using System.Windows.Input;

namespace WA4D0G.ViewModel.Commands
{
    public abstract class MainViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        protected MainViewModel _mainViewModel;

        public MainViewModelCommand(MainViewModel viewModel)
        {
            _mainViewModel = viewModel;
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
