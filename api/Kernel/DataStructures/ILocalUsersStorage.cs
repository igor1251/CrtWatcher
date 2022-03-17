using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface ILocalUsersStorage
    {
        Task<List<User>> LoadCertificateSubjectsAndCertificates();
        Task InsertCertificate(ICertificate certificate);
    }
}
