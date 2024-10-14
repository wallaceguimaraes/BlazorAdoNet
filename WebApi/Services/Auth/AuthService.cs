using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using WebApi.Extensions;
using WebApi.Models.Entities.Auth;
using WebApi.Models.Entities.Users;
using WebApi.Models.ViewModel;
using WebApi.Repositories.Users;

namespace WebApi.Services.Auth
{
        public class AuthService : IAuthService
        {
        private readonly IUserRepository _userRepository;
        private const string USER_NOT_FOUND = "USER_NOT_FOUND";
        private const string WRONG_PASSWORD = "WRONG_PASSWORD";
        private const string AUTHENTICATION_ERROR = "AUTHENTICATION_ERROR";


        public AuthService(IUserRepository userRepository,
                           IOptions<AuthOptions> authOptions)
        {
            _userRepository = userRepository;
        }

        public async Task<(User? user, string? error)> Authenticate(LoginModel loginModel)
        {
            try
            {             
                var user = await _userRepository.GetUserByEmail(loginModel.Email);
                
                if (user == null) 
                    return (null, USER_NOT_FOUND);

                var wrongPassword = user.Password != loginModel.Password.Encrypt(user.Salt);

                if (wrongPassword)
                    return (null, WRONG_PASSWORD);
                    
                return (user, null);
            }
            catch(Exception)
            {
                // log exception
                return (null,AUTHENTICATION_ERROR);
            }
        }

        public async Task UpdateTokenVersion(long userId)
        {    
            try
            {
                long tokenVersion = await _userRepository.GetTokenVersion(userId);
                var versionUpdated = tokenVersion + 1;

                await _userRepository.UpdateTokenVersion(userId, versionUpdated);
                
            }
            catch (System.Exception)
            {
                //log error
                return;
            }
        }

        public async Task ValidateTokenVersion(TokenValidatedContext context)
        {
            try
            {
                var userId = context.Principal.FindFirst(ApiClaimTypes.UserId)?.Value;
                var tokenVersionClaim = context.Principal.FindFirst("Version")?.Value;

                if (long.TryParse(userId, out var userIdParsed) && int.TryParse(tokenVersionClaim, out var tokenVersionParsed))
                {
                    var token = await _userRepository.GetTokenVersion(userIdParsed);
                    if (token != tokenVersionParsed)
                        context.Fail("Token inválido, versão de token incorreta.");
                }
                else
                {
                    context.Fail("Token inválido, versão de token não encontrada.");
                }
            }
            catch (System.Exception)
            {
                //log error
                return;
            }
        }

    }
}
