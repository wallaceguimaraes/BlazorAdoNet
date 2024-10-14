
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Models.Entities.Users;
using WebApi.Models.ViewModel;

namespace WebApi.Services.Auth
{
    public interface IAuthService
    {
        Task<(User? user, string? error)> Authenticate(LoginModel loginModel);
        Task UpdateTokenVersion(long userId);
        Task ValidateTokenVersion(TokenValidatedContext context);
    }
}