
using System.Text;
using System.Text.Json;
using BlazorApp.Providers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using BlazorApp.Models.Results.Errors;
using BlazorApp.Models.Results.Login;
using System.Net.Http.Headers;
using BlazorApp.Models.ViewModels.Login;
using BlazorApp.Models.IntegrationModel;
using Microsoft.Extensions.Options;


namespace BlazorApp.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly WebApiOptions _webApiOptions;
        private readonly IConfiguration _config;



        public AuthService(HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage,
        IOptions<WebApiOptions> webApiOptions,
         IConfiguration config
         )
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _webApiOptions = webApiOptions.Value;
            _config = config;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var loginJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync($"{_webApiOptions.BaseUrl}/{_webApiOptions.Authenticate}", new StringContent(loginJson, Encoding.UTF8, "application/json"));

            LoginResult loginResult = new LoginResult
            {
                Successful = false,
                Error = "ERROR",
            };

            if (response is null)
                return loginResult;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    var badRequest = JsonSerializer.Deserialize<BadRequest>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    loginResult = new LoginResult
                    {
                        Successful = false,
                        Error = badRequest.Errors.First(),
                    };
                    break;

                case HttpStatusCode.UnprocessableEntity:
                    var unprocessableEntity = JsonSerializer.Deserialize<UnprocessableEntity>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    loginResult = new LoginResult
                    {
                        Successful = false,
                        Error = unprocessableEntity.Error,
                    };
                break;

                case HttpStatusCode.OK:
                    var result = JsonSerializer.Deserialize<TokenResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    loginResult = new LoginResult
                    {
                        Successful = true,
                        Error = null,
                        Token = result.Token
                    };

                    await _localStorage.SetItemAsync("authToken", loginResult.Token);

                    ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", loginResult.Token);

                    break;
            }

            return loginResult;
        }

        public async Task Logout()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await _httpClient.PostAsync($"{_webApiOptions.BaseUrl}/{_webApiOptions.Logout}", new StringContent("{}", Encoding.UTF8, "application/json"));
            
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;

        }
    }
}