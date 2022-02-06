namespace WA4D0G.ViewModel.Commands
{
    public class DeleteCertificateCommand : MainViewModelCommand
    {
        public DeleteCertificateCommand(MainViewModel viewModel) : base(viewModel) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            _mainViewModel.DeleteCertificate();
        }
    }
}
