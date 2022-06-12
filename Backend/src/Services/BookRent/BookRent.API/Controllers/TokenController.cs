using System.ComponentModel.DataAnnotations;
using Auth;
using ErrorHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookRent.Services;
using BookRent.DTOs.Inputs;

namespace BookRent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService TokenService;

        public TokenController(ITokenService tokenService)
        {
            this.TokenService = tokenService;
        }

        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(JsonWebToken))]
        public async Task<IActionResult> Login([Required][FromBody] LoginParam input)
        {
            try
            {
                var result = await TokenService.LoginAsync(input);
                return Ok(result);
            }
            catch (UnauthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([Required][FromBody] LogoutParam input)
        {
            var userID = HttpContext.User.Claims.Where(x => x.Type == "userid").SingleOrDefault();
            await TokenService.LogoutAsync(input);
            return Ok();
        }
    }
}
