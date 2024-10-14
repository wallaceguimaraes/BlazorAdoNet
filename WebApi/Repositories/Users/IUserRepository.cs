using WebApi.Models.Entities.Users;

namespace WebApi.Repositories.Users
{
        public interface IUserRepository
        {
            Task<User> GetUserByEmail(string username);
            Task AddUser(User user);
            Task<long> GetTokenVersion(long userId);
            Task<bool> UpdateTokenVersion(long userId, long token);
            Task<User> GetUserById(long userId);

    }
}

