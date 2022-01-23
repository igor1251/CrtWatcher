using ElectrnicDigitalSignatire.Models.Classes;

namespace WA4D0GWebPanel.Models
{
    public class SubjectDetailsViewModel
    {
        private CertificateSubject _subject;

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
