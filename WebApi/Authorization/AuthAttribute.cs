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
        }
    }
}