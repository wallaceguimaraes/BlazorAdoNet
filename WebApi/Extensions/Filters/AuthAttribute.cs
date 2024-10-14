// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Net.Http.Headers;
// using api.Extensions.Http;
// using api.Models.IntegrationModel.Account;
// using System;
// using System.Threading.Tasks;
// using api.Models.IntegrationModel.Account.AccessControl;
// using Microsoft.Extensions.Options;
// using api.Models.IntegrationModel;
// using System.Net;
// using Microsoft.AspNetCore.Http;

// namespace api.Filters
// {
//     public class AuthAttribute : Attribute, IAsyncAuthorizationFilter
//     {
//         private const string Service = "Notifications";
//         private readonly string _scope;
//         private readonly bool _requiredSecret;

//         public AuthAttribute(string scope)
//         {
//             _scope = scope;
//         }

//         public AuthAttribute(string scope = null, bool requiredSecret = false)
//         {
//             _scope = scope;
//             _requiredSecret = requiredSecret;
//         }

//         public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//         {
//             var auth = context.HttpContext.RequestServices.GetRequiredService<IOptions<AuthOptions>>();
//             var secret = context.HttpContext.Request.Query["secret"].ToString();

//             if (_requiredSecret)
//             {
//                 if (!SecretValid(auth.Value, secret))
//                 {
//                     context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
//                     context.HttpContext.Response.Clear();
//                 }

//                 return;
//             }

//             if (context.HttpContext.Request.Headers.ContainsKey(HeaderNames.Authorization))
//             {
//                 var accountApi = context.HttpContext.RequestServices.GetRequiredService<IAccountApi>();
//                 var token = context.HttpContext.Request.Headers[HeaderNames.Authorization];
//                 var permission = new Permission { Service = Service, Scope = _scope };


//                 var whoAmI = await accountApi.WhoAmI(token, permission);
//                 var accessGranted = whoAmI?.AccessGranted ?? false;

//                 if (accessGranted)
//                 {
//                     context.HttpContext.SetWhoAmI(whoAmI);
//                     return;
//                 }
//             }

//             context.Result = new UnauthorizedResult();
//         }

//         private bool SecretValid(AuthOptions options, string secretReceived)
//         {
//             if (!string.IsNullOrEmpty(secretReceived) && (options?.Secret == secretReceived))
//                 return true;

//             return false;
//         }
//     }
// }