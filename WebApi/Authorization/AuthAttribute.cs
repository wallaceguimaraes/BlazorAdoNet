using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Authorization
{
    public class AuthAttribute : ActionFilterAttribute
    {
        private string _scope;
        private bool _requiredSecret;

        public AuthAttribute() { }

        public AuthAttribute(string scope = null, bool requiredSecret = false, bool webhookSecret = false, bool requiredSuperAdmin = false)
        {
            _scope = scope;
            _requiredSecret = requiredSecret;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {
            // var authOptions = actionContext.HttpContext.RequestServices.GetService<IOptions<AuthOptions>>();
            // var dbContext = actionContext.HttpContext.RequestServices.GetService<ApiDbContext>();
            var claims = actionContext.HttpContext.User.Claims;
            var secret = actionContext.HttpContext.Request.Query["secret"].ToString();
            // var userAuthorization = new UserAuthorization(dbContext);

            // if (_requiredSecret || !string.IsNullOrEmpty(secret))
            // {
            //     if (secret == authOptions.Value.Secret)
            //     {
            //         await next();
            //     }

            //     actionContext.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            //     return;
            // }




            // await userAuthorization.FindUser(claims.UserId(), claims.Salt(), claims.HolderId());

            // if (userAuthorization.UserNotFound || !userAuthorization.User.Active || userAuthorization.User.NeedChangePassword)
            // {
            //     actionContext.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            //     return;
            // }

            // var holderAsEmployee = claims.HolderAsEmployee();
            // await userAuthorization.CheckPermission(actionContext.HttpContext.User.Claims.ApplicationId(), _scope, holderAsEmployee: holderAsEmployee, requiredSuperAdmin: _requiredSuperAdmin);

            // if (userAuthorization.AccessGranted)
            // {
            //     await next();
            // }
            // else
            // {
            //     actionContext.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            // }
        }
    }
}