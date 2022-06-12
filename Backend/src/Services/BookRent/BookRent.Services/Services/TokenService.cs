using Auth;
using Database.Models;
using ErrorHandling;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BookRent.DTOs.Inputs;
using models = Database.Models;

namespace BookRent.Services
{
    public class TokenService : ITokenService
    {
        private readonly DatabaseContext DB;
        private readonly IJwtHandler JwtHandler;
        private readonly IConfiguration Configuration;
        private readonly SignInManager<models.USR.AppUser> SignInManager;

        public TokenService(IConfiguration configuration, IJwtHandler jwtHandler, DatabaseContext db, SignInManager<models.USR.AppUser> signinManager)
        {
            this.JwtHandler = jwtHandler;
            this.DB = db;
            this.Configuration = configuration;
            this.SignInManager = signinManager;
        }

        public async Task<JsonWebToken> LoginAsync(LoginParam input)
        {
            if (input.grant_type == "password")
            {
                var user = DB.Users.Where(o => o.UserName == input.username).FirstOrDefault();
                if (user != null)
                {
                    if (!user.EmailConfirmed)
                        throw new UnauthorizedException("Please verifly your email account before start login.");

                    var signInResult = await SignInManager.CheckPasswordSignInAsync(user, input.password, false);
                    if (signInResult.Succeeded)
                    {
                        string refreshToken = await GenerateNewRefreshTokenAsync(user.UserName);
                        return JwtHandler.Create(user.UserName, user.FullName, refreshToken, false);
                    }
                    else
                    {
                        throw new UnauthorizedException("Invalid password.");
                    }
                }
                else
                {
                    throw new UnauthorizedException("User not found.");
                }
            }
            else if (input.grant_type == "refresh_token")
            {
                var token = await DB.RefreshTokens.FirstOrDefaultAsync(o => o.Token == input.refresh_token);
                if (token == null)
                    throw new UnauthorizedException("Refresh token not found");

                if (token.ExpireDate < DateTime.UtcNow)
                    throw new UnauthorizedException("Refresh token has been expired");

                var refreshToken = await GenerateNewRefreshTokenAsync(token.Username);
                DB.Remove(token);
                await DB.SaveChangesAsync();

                var user = DB.Users.Where(o => o.UserName == token.Username).FirstOrDefault();
                if (user != null)
                {
                    return JwtHandler.Create(user.UserName, user.FullName, refreshToken, false);
                }
                else
                {
                    throw new UnauthorizedException("User not found.");
                }
            }
            else
            {
                throw new UnauthorizedException("Invalid grant type (must be \"password\" or \"refresh_token\")");
            }
        }

        public async Task LogoutAsync(LogoutParam input)
        {
            var token = await DB.RefreshTokens.FirstOrDefaultAsync(o => o.Token == input.RefreshToken);
            if (token != null)
            {
                DB.Remove(token);
                await DB.SaveChangesAsync();
            }
        }

        private async Task<string> GenerateNewRefreshTokenAsync(string username)
        {
            var token = new models.USR.RefreshToken();
            token.Token = Guid.NewGuid().ToString("N");
            token.Username = username;
            token.ExpireDate = DateTime.UtcNow.AddDays(14);
            await DB.AddAsync(token);
            await DB.SaveChangesAsync();
            return token.Token;
        }
    }
}