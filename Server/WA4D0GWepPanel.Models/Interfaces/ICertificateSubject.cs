using WA4D0GWebPanel.Models.Classes;
using System.Collections.Generic;

namespace WA4D0GWebPanel.Models.Interfaces
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
