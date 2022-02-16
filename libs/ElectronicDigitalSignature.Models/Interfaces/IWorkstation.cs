using ElectronicDigitalSignatire.Models.Interfaces;
using System.Collections.Generic;

namespace ElectronicDigitalSignature.Models.Interfaces
{
    public interface IWorkstation
    {
        string IP { get; set; }
        IEnumerable<ICertificateSubject> SubjectsList { get; set; }
    }
}
