using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using X509Observer.Identity.Entities;
using X509Observer.Reporters;
using System.Linq;
using X509Observer.Identity.Repositories;

namespace X509Observer.Identity.MaintenanceTools
{
    public class JwtTokensOperator
    {
        private IApiUsersRepository _apiUsersRepository;

        public JwtTokensOperator(IApiUsersRepository apiUsersRepository)
        {
            _apiUsersRepository = apiUsersRepository;
        }

        public Task<string> Generate(ApiUser user, int keyValidityPeriod, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("ID", user.ID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(keyValidityPeriod),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public async Task<ApiUser> ValidateAsync(string token, string secret)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secret);
                var validationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                });
                var userID = int.Parse(((JwtSecurityToken)validationResult.SecurityToken).Claims.First(x => x.Type == "ID").Value);
                //var founedUser = await _apiUsersRepository.GetApiUserByIDAsync(userID);
                //founedUser.Role = "administrator";
                return await _apiUsersRepository.GetApiUserByIDAsync(userID);
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("Validate(string token)", ex);
                return null;
            }
        }
    }
}
