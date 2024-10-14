using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Models.ViewModel;
using WebApi.Results.Errors;
using WebApi.Utils;
using WebApi.Models.Results.Success;
using WebApi.Models.Entities.Auth;
using Microsoft.Extensions.Options;
using WebApi.Extensions.Identity;
using WebApi.Authorization;
using WebApi.Services.Auth;

namespace AuthenticationApp.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApiToken _apiToken;
        private readonly AuthOptions _authOptions;

        public AuthController(IAuthService authService, IOptions<AuthOptions> authOptions)
        {
            _authService = authService;
            _apiToken = new ApiToken(authOptions.Value);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromBody] LoginModel loginModel)
        {
            var (user, error) = await _authService.Authenticate(loginModel);

            if (!string.IsNullOrEmpty(error))
                return new UnprocessableEntityJson(error);

            user.TokenVersion ++;
            var token = new TokenResult(_apiToken.GenerateTokenForUser(user), _apiToken.ExpiresIn);
            await _authService.UpdateTokenVersion(user.Id);

            return token;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.User.Claims.UserId();
            await _authService.UpdateTokenVersion(userId);

            return NoContent();
        }

        [HttpGet("wake-up")]
        public async Task<IActionResult> WakeUp()
        {
            return Ok("Server online!");
        }
    }
}
