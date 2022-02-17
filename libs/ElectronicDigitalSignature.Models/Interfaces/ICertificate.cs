using System;

namespace ElectronicDigitalSignatire.Models.Interfaces
{
    public interface ICertificate
    {
        int ID { get; set; }
        string CertificateHash { get; set; }
        string Algorithm { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}
