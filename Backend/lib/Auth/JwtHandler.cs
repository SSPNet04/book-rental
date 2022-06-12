using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions _options;

        public JwtHandler(JwtOptions options)
        {
            _options = options;
        }

        public ClientJsonWebToken ClientCreate(string userId, string displayName)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = null,            // Not required as no third-party is involved
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(365),
                Subject = new ClaimsIdentity(new List<Claim> {
                new Claim("userid", userId.ToString()),
                new Claim("authtype", "client")
            }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256)
            };
            var jwt = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);
            var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)(new TimeSpan(tokenDescriptor.Expires.Value.Ticks - centuryBegin.Ticks).TotalSeconds);
            var expIn = (long)TimeSpan.FromDays(365).TotalSeconds;
            return new ClientJsonWebToken
            {
                token = token,
                expires = exp,
                expires_in = expIn,
                display_name = displayName
            };
        }

        public JsonWebToken Create(string username, string displayName, string? refreshToken = null, bool mustChangePassword = false)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = null,            // Not required as no third-party is involved
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
                Subject = new ClaimsIdentity(new List<Claim> {
                new Claim("userid", username.ToString()),
                new Claim("authtype", "user"),
                new Claim("mustChangePassword", mustChangePassword.ToString()),
            }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256)
            };
            var jwt = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);
            var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)(new TimeSpan(tokenDescriptor.Expires.Value.Ticks - centuryBegin.Ticks).TotalSeconds);
            var expIn = (long)TimeSpan.FromMinutes(_options.ExpiryMinutes).TotalSeconds;
            return new JsonWebToken
            {
                token = token,
                expires = exp,
                expires_in = expIn,
                refresh_token = refreshToken,
                user_id = username,
                display_name = displayName,
                mustChangePassword = mustChangePassword
            };
        }
    }
}
