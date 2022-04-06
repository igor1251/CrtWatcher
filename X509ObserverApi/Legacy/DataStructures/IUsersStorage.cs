using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface IUsersStorage
    {
        Task<List<User>> GetUsers();
        Task<User> GetUserByID(int id);
        Task<List<Certificate>> GetCertificates(int subjectID);
        Task DeleteCertificate(int certificateID);
        Task DeleteUser(int subjectID);
        Task InsertUser(User subject);
        Task InsertUser(List<User> subjects);
        Task InsertCertificate(Certificate certificate, int subjectID);
        Task UpdateUser(User subject);
    }
}
