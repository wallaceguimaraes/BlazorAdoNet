using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;
using WebApi.Extensions.Identity;
using WebApi.Services.Users;

namespace WebApi.Authorization
{
    public class AuthAttribute : ActionFilterAttribute
    {

        public AuthAttribute() { }

        public override async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {
            var userService = actionContext.HttpContext.RequestServices.GetService<IUserService>();
            
            var claims = actionContext.HttpContext.User.Claims;
            var (user, error) = await userService.FindUserAuthenticated(claims.UserId(), claims.Salt());

            if (user == null || !string.IsNullOrEmpty(error))
            {
                actionContext.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }

            await next();
        }
    }
}