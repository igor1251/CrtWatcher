namespace WA4D0G.ViewModel.Commands
{
    public class SaveCertificatesToDatabaseCommand : MainViewModelCommand
    {
        public SaveCertificatesToDatabaseCommand(MainViewModel viewModel) : base(viewModel) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            _mainViewModel.SaveExtractedCertificatesToDatabase();
        }
    }
}
