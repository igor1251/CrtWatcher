using System.Collections.Generic;

namespace DataStructures
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
