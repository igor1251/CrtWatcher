using ElectronicDigitalSignatire.Models.Classes;
using System.Collections.Generic;

namespace ElectronicDigitalSignatire.Models.Interfaces
{
    public interface ICertificateSubject
    {
        int ID { get; set; }
        string SubjectName { get; set; }
        string SubjectPhone { get; set; }
        string SubjectComment { get; set; }
        List<CertificateData> CertificateList { get; set; }
    }
}
