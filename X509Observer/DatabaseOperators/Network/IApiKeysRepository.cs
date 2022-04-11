using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X509Observer.Primitives.Network;

namespace X509Observer.DatabaseOperators.Network
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
