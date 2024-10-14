
using BlazorApp.Models.Results.Register;
using BlazorApp.Models.ViewModels.Register;

namespace BlazorApp.Services.User
{
    public interface IUserService
    {
        Task<RegisterResult> Register(RegisterModel registerModel);
    }
}