using ElectrnicDigitalSignatire.Models.Classes;
using System.Collections.Generic;

namespace ElectrnicDigitalSignatire.Models.Interfaces
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
