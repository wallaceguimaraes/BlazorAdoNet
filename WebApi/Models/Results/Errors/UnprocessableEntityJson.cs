using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Results.Errors
{
    public class UnprocessableEntityJson : IActionResult
    {
        public UnprocessableEntityJson() { }

        public UnprocessableEntityJson(string error)
        {
            Error = error;
        }

        public string Error { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var json = new JsonResult(this) { StatusCode = 422 };
            await json.ExecuteResultAsync(context);
        }
    }
}