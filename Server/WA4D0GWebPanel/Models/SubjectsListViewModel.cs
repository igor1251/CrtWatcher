using ElectrnicDigitalSignatire.Models.Classes;
using ElectrnicDigitalSignatire.Services.Interfaces;
using System.Collections.Generic;

namespace WA4D0GWebPanel.Models
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
