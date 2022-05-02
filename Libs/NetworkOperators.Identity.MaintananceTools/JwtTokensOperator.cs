using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NetworkOperators.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Tools.Reporters;
using NetworkOperators.Identity.Repositories;

namespace NetworkOperators.Identity.MaintananceTools
{
    public class JwtTokensOperator
    {
        private IUsersRepository _apiUsersRepository;

        public JwtTokensOperator(IUsersRepository apiUsersRepository)
        {
            _apiUsersRepository = apiUsersRepository;
        }

        public Task<string> Generate(User user, int keyValidityPeriod, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Username", user.UserName.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(keyValidityPeriod),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public async Task<User> ValidateAsync(string token, string secret)
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
                if (validationResult != null)
                {
                    if (validationResult.IsValid)
                    {
                        var username = ((JwtSecurityToken)validationResult.SecurityToken).Claims.First(x => x.Type == "Username").Value;
                        return await _apiUsersRepository.GetUserByUsernameAsync(username);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("Validate(string token)", ex);
                return null;
            }
        }
    }
}
