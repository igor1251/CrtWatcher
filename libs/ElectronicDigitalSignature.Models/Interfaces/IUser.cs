using ElectronicDigitalSignatire.Models.Classes;
using System.Collections.Generic;

namespace ElectronicDigitalSignatire.Models.Interfaces
{
    public interface IUser
    {
        int ID { get; set; }
        string UserName { get; set; }
        string UserPhone { get; set; }
        string UserComment { get; set; }
        List<Certificate> CertificateList { get; set; }
    }
}
