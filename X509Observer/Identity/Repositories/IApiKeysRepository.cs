using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.Identity.Entities;

namespace X509Observer.Identity.Repositories
{
    public interface IApiKeysRepository
    {
        Task<ApiKey> GetApiKeyByValueAsync(string value);
        Task<List<ApiKey>> GetApiKeysAsync();
        Task AddApiKeyAsync(ApiKey apiKey);
        Task<ApiKey> GenerateApiKeyAsync(ApiUser apiUser);
        Task RemoveApiKeyAsync(ApiKey apiKey);
    }
}
