using Newtonsoft.Json;
using WebApi.Models.Entities.Users;
using Required = WebApi.Validations.Required;

namespace WebApi.Models.ViewModel
{
    public class UserRegisterModel
    {

        [JsonProperty("name"), Required]
        public string Name { get; set; }

        [JsonProperty("email"), Required]
        public string Email { get; set; }
        
        [JsonProperty("password"), Required]
        public string Password { get; set; }


        public User Map(string salt, string passwordEncrypt)
        {
            return new User
            {
                Name = Name,
                Email = Email,
                Password = passwordEncrypt,
                Salt = salt,
                TokenVersion = 0
            };
        }
    }
}