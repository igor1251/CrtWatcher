using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X509Observer.Primitives.Database;
using X509Observer.Primitives.Network;
using System.Security.Cryptography;
using System.Text.Json;

namespace X509Observer.DatabaseOperators.Network
{
    public class ApiKeysRepository : IApiKeysRepository
    {
        private IDbContext _dbContext;

        public ApiKeysRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddApiKeyAsync(ApiKey apiKey)
        {
            throw new NotImplementedException();
        }

        public Task<ApiKey> GenerateApiKeyAsync(ApiUser apiUser)
        {
            var random = new Random();
            var inputString = new StringBuilder();
            inputString.Append(JsonSerializer.Serialize<ApiUser>(apiUser));
            var salt = DateTime.Now.ToString() + random.Next(10, 100000).ToString();
            inputString.Append(salt);
            byte[] bytes = MD5.HashData(Encoding.ASCII.GetBytes(inputString.ToString()));
            var apiKey = new ApiKey()
            {
                Value = Convert.ToHexString(bytes),
                ExpirationTime = DateTime.Now.AddDays(365)
            };
            return Task.FromResult(apiKey);
        }

        public Task<ApiKey> GetApiKeyByValueAsync(string value)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApiKey>> GetApiKeysAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveApiKeyAsync(ApiKey apiKey)
        {
            throw new NotImplementedException();
        }
    }
}
