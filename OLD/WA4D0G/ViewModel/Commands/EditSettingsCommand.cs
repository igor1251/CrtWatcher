namespace WA4D0G.ViewModel.Commands
{
    internal class EditSettingsCommand : MainViewModelCommand
    {
        public EditSettingsCommand(MainViewModel viewModel) : base(viewModel) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            _mainViewModel.EditSettings();
        }
    }
}
