using ECommerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings jwtOptions;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
        }
        public string CreateToken(string userId, string email, string userName, IReadOnlyList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var seckey = jwtOptions.SecretKey;
            if (string.IsNullOrWhiteSpace(seckey))
                throw new InvalidOperationException("Secret Key Missing !");

            if (seckey.Length < 32)
                throw new InvalidOperationException("Secret Key too short !");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(seckey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // HS256

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class JwtSettings
        {
            public string SecretKey { get; set; } = default!;
            public string Issuer { get; set; } = default!;
            public string Audience { get; set; } = default!;
            public int ExpirationMinutes { get; set; } = default!;
        }
    }
}
