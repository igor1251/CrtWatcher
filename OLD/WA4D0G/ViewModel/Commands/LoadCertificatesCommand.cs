namespace WA4D0G.ViewModel.Commands
{
    internal class LoadCertificatesCommand : MainViewModelCommand
    {
        public LoadCertificatesCommand(MainViewModel viewModel) : base(viewModel) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            _mainViewModel.LoadCertificates();
        }
    }
}
