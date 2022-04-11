using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.Primitives.Network;

namespace X509Observer.DatabaseOperators.Basic
{
    public interface IApiUsersRepository
    {
        Task<ApiUser> GetApiUserByIDAsync(int userID);
        Task<List<ApiUser>> GetApiUsersAsync();
        Task AddApiUserAsync(ApiUser user);
        Task UpdateApiUserAsync(ApiUser user);
        Task RemoveApiUserAsync(int userID);
    }
}
