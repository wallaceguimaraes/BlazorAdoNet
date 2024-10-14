using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models.Entities.Auth;
using WebApi.Models.Entities.Users;

namespace WebApi.Utils
{
    public class ApiToken
    {
        private readonly AuthOptions _options;

        public ApiToken(AuthOptions options)
        {
            if (string.IsNullOrEmpty(options.Key))
            {
                throw new ArgumentNullException(nameof(options.Key));
            }
          
            _options = options;
        }

        private byte[] EncodedKey => Encoding.ASCII.GetBytes(_options.Key);
        public DateTime ExpiresIn { get; set; }
        public SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(EncodedKey);
        public SigningCredentials SigningCredentials => new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);


        public string GenerateTokenForUser(User user)
        {
            ExpiresIn = DateTime.UtcNow.AddHours(_options.ExpireTokenIn);

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken
            (
                issuer: _options.Issuer,
                audience: _options.Audience,
                notBefore: DateTime.UtcNow,
                signingCredentials: SigningCredentials,
                expires: ExpiresIn,
                claims: new[] {
                    new Claim(ClaimTypes.Actor, "User"),
                    new Claim(ApiClaimTypes.UserId, user.Id.ToString()),
                    new Claim(ApiClaimTypes.Email, user.Email),
                    new Claim(ApiClaimTypes.Salt, user.Salt),
                    new Claim(ApiClaimTypes.Version, user.TokenVersion.ToString())
                }
            ));
        }
    }
}