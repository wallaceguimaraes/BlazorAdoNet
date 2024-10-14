using WebApi.Extensions;
using WebApi.Infrastructure.Security;
using WebApi.Models.Entities.Users;
using WebApi.Models.ViewModel;
using WebApi.Repositories.Users;

namespace WebApi.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private const string CREATE_USER_ERROR = "CREATE_USER_ERROR";
        private const string USER_ALREADY_CREATED = "USER_ALREADY_CREATED";
        private const string USER_NOT_FOUND = "USER_NOT_FOUND";


        public UserService(IUserRepository unitOfWork
        )
        {
            userRepository = unitOfWork;
        }

        public async Task<(User? user, string? error)> Create(UserRegisterModel model)
        {
            User user;
            
            try
            {
                string salt = new Salt().ToString();
                string passwordEncrypt = model.Password.Encrypt(salt);
                user = model.Map(salt, passwordEncrypt);

                if(await userRepository.GetUserByEmail(model.Email) != null)
                    return (null, USER_ALREADY_CREATED);
                
                await userRepository.AddUser(user);
            }
            catch (Exception){
                //create error exception log here
                return (null, CREATE_USER_ERROR);
            }

            return (user, null);
        }

        public async Task<(User? user, string? error)> GetUserById(long userId)
        {
            try
            {
                var user = await userRepository.GetUserById(userId);
                return (user, null);
            }
            catch (Exception)
            {
                return (null, USER_NOT_FOUND);
            }
        }

    }
}