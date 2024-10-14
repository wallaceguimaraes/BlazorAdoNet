
using BlazorApp.Models.Results.Login;
using BlazorApp.Models.ViewModels.Login;

namespace BlazorApp.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
    }
}