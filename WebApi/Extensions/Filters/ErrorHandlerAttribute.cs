using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Extensions.Filters
{
    public class ErrorHandlerAttribute : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ErrorHandlerAttribute> _logger;

        public ErrorHandlerAttribute(IWebHostEnvironment env, ILogger<ErrorHandlerAttribute> logger)
        {
            _env = env;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (!_env.IsDevelopment())
            {
                _logger.LogError(context.Exception, "Error");
            }

            context.Result = new StatusCodeResult(500);
        }
    }
}
