namespace WA4D0G.ViewModel.Commands
{
    internal class UpdateCertificateCommand : MainViewModelCommand
    {
        public UpdateCertificateCommand(MainViewModel viewModel) : base(viewModel) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            _mainViewModel.EditSelectedCertificate();
        }
    }
}
