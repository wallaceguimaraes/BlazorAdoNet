using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Models.ViewModel;
using WebApi.Results.Errors;
using WebApi.Utils;
using WebApi.Models.Results.Success;
using WebApi.Models.Entities.Auth;
using Microsoft.Extensions.Options;
using WebApi.Services.Users;

namespace AuthenticationApp.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterModel model)
        {
            var (user, error) = await _userService.Create(model);

            if (!string.IsNullOrEmpty(error))
                return new UnprocessableEntityJson(error);
            
            return new TokenResult();        
        }
    }
}
