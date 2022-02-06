using ElectronicDigitalSignatire.Models.Classes;

namespace WA4D0GWebPanel.ViewModels
{
    public class SubjectDetailsViewModel
    {
        private CertificateSubject _subject;

        public SubjectDetailsViewModel()
        {
            _subject = new CertificateSubject();
        }

        public SubjectDetailsViewModel(CertificateSubject subject)
        {
            _subject = subject;
        }

        public CertificateSubject Subject 
        {
            get => _subject;
            set => _subject = value;
        }
    }
}
