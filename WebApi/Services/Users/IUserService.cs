using WebApi.Models.Entities.Users;
using WebApi.Models.ViewModel;

namespace WebApi.Services.Users
{
    public interface IUserService
    {
        Task<(User? user, string? error)> Create(UserRegisterModel registerModel);
        Task<(User? user, string? error)> GetUserById(long userId);
        Task<(User? user, string? error)> FindUserAuthenticated(long userId, string salt);

    }
}