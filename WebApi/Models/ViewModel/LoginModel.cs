
using Newtonsoft.Json;
using Required = WebApi.Validations.Required;

namespace WebApi.Models.ViewModel
{
        public class LoginModel
        {
            [JsonProperty("email"), Required]
            public string Email { get; set; }
            
            [JsonProperty("password"), Required]
            public string Password { get; set; }
        }

}