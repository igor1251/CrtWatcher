using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface ILocalUsersStore
    {
        Task<List<User>> LoadCertificateSubjectsAndCertificates();
        Task InsertCertificate(ICertificate certificate);
    }
}
