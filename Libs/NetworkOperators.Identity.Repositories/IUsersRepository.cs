using NetworkOperators.Identity.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetworkOperators.Identity.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetUserByIDAsync(int userID);
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByAuthenticationDataAsync(string username, string passwordHash);
        Task<int> GetUserIDAsync(string username);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task RemoveUserAsync(int userID);
        Task<bool> IsUserExistsAsync(User user);
    }
}
