using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth;
using BookRent.DTOs.Inputs;

namespace BookRent.Services
{
    public interface ITokenService
    {
        Task<JsonWebToken> LoginAsync(LoginParam input);
        Task LogoutAsync(LogoutParam input);
    }
}
