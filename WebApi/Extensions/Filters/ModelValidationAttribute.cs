using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Results.Errors;
using WebApi.Infrastructure.Mvc;

namespace WebApi.Extensions.Filters
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Any(arg => arg.Value == null))
            {
                context.Result = new BadRequestJson("Request body cannot be blank.");
            }
            else if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => new ModelErrorMessage(e))
                    .ToList();

                context.Result = new BadRequestJson(errors);
            }
        }
    }
}