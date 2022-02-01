using ElectrnicDigitalSignatire.Models.Classes;
using System.Collections.Generic;

namespace WA4D0GWebPanel.ViewModels
{
    public class SubjectsListViewModel
    {
        public List<CertificateSubject> CertificateSubjects { get; set; }

        public SubjectsListViewModel(List<CertificateSubject> subjects)
        {
            CertificateSubjects = subjects;
        }
    }
}
