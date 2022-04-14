using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.Identity.Entities;

namespace X509Observer.Identity.Repositories
{
    public interface IApiUsersRepository
    {
        Task<ApiUser> GetApiUserByIDAsync(int userID);
        Task<List<ApiUser>> GetApiUsersAsync();
        Task<ApiUser> GetApiUserByUserNameAsync(string username);
        Task<int> GetApiUserIDAsync(string username);
        Task AddApiUserAsync(ApiUser user);
        Task UpdateApiUserAsync(ApiUser user);
        Task RemoveApiUserAsync(int userID);
    }
}
