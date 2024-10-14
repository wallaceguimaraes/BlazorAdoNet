using System.Net;
using System.Text;
using System.Text.Json;
using BlazorApp.Models.IntegrationModel;
using BlazorApp.Models.Results.Errors;
using BlazorApp.Models.Results.Register;
using BlazorApp.Models.ViewModels.Register;
using Microsoft.Extensions.Options;

namespace BlazorApp.Services.User
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly WebApiOptions _webApiOptions;


        public UserService(HttpClient httpClient, IOptions<WebApiOptions> webApiOptions)
        {
            _httpClient = httpClient;
            _webApiOptions = webApiOptions.Value;

        }

        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            var modelJson = JsonSerializer.Serialize(registerModel);
            var response = await _httpClient.PostAsync($"{_webApiOptions.BaseUrl}/{_webApiOptions.CreateUser}", new StringContent(modelJson, Encoding.UTF8, "application/json"));

            RegisterResult registerResult = new RegisterResult
            {
                Successful = false,
                Error = "ERROR",
            };

            if (response is null)
                return registerResult;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    var badRequest = JsonSerializer.Deserialize<BadRequest>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    registerResult = new RegisterResult
                    {
                        Successful = false,
                        Error = badRequest.Errors.First(),
                    };
                    break;

                case HttpStatusCode.UnprocessableEntity:
                    var unprocessableEntity = JsonSerializer.Deserialize<UnprocessableEntity>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    registerResult = new RegisterResult
                    {
                        Successful = false,
                        Error = unprocessableEntity.Error,
                    };
                    break;

                case HttpStatusCode.OK:
                    registerResult = new RegisterResult
                    {
                        Successful = true,
                        Error = null,
                    };

                    break;
            }

            return registerResult;
        }
    }
}