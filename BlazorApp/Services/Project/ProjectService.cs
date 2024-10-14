
using System.Text;
using System.Text.Json;
using BlazorApp.Models.Results;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp.Models.IntegrationModel;
using Microsoft.Extensions.Options;
using System.Net;
using BlazorApp.Models.Results.Errors;
using System.Net.Http.Headers;
using BlazorApp.Models.Results.Project;
using BlazorApp.Models.ViewModels.Project;


namespace BlazorApp.Services.Project
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly WebApiOptions _webApiOptions;


        public ProjectService(HttpClient httpClient,
        ILocalStorageService localStorage,
        IOptions<WebApiOptions> webApiOptions)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _webApiOptions = webApiOptions.Value;
        }

        public async Task<Result> DeleteProject(long projectId)
        {
            await GetToken();

            var result = new Result { Successful = true };
            var response = await _httpClient.DeleteAsync($"{_webApiOptions.BaseUrl}/{_webApiOptions.DeleteProject}/{projectId}");

            if (response.IsSuccessStatusCode)
                return result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    var badRequest = JsonSerializer.Deserialize<BadRequest>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    result.Successful = false;
                    result.Error = badRequest.Errors.First();

                    break;

                case HttpStatusCode.UnprocessableEntity:
                    var unprocessableEntity = JsonSerializer.Deserialize<UnprocessableEntity>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    result.Successful = false;
                    result.Error = unprocessableEntity.Error;

                    break;

                case HttpStatusCode.Unauthorized:

                    result.Successful = false;
                    result.Error = "UNAUTHORIZED";

                    break;
            }

            return result;
        }

        public async Task<List<ProjectResult>> GetProjects(FindProjectModel model)
        {
            await GetToken();

            var modelJson = JsonSerializer.Serialize(model);
            var response = await _httpClient.PostAsync($"{_webApiOptions.BaseUrl}/{_webApiOptions.ListProjects}", new StringContent(modelJson, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var projects = JsonSerializer.Deserialize<List<ProjectResult>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return projects;
            }

            return new List<ProjectResult>();
        }

        private async Task GetToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }


        public async Task<Result> AddProject(ProjectModel model)
        {
            await GetToken();

            var result = new Result { Successful = true };

            var modelJson = JsonSerializer.Serialize(model);
            var response = await _httpClient.PostAsync($"{_webApiOptions.BaseUrl}/{_webApiOptions.CreateProject}", new StringContent(modelJson, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                return result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    var badRequest = JsonSerializer.Deserialize<BadRequest>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    result.Successful = false;
                    result.Error = badRequest.Errors.First();

                    break;

                case HttpStatusCode.UnprocessableEntity:
                    var unprocessableEntity = JsonSerializer.Deserialize<UnprocessableEntity>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    result.Successful = false;
                    result.Error = unprocessableEntity.Error;

                    break;
                case HttpStatusCode.Unauthorized:

                    result.Successful = false;
                    result.Error = "UNAUTHORIZED";

                    break;
            }

            return result;
        }


        public async Task<ProjectResult> GetProjectById(long projectId)
        {
            await GetToken();

            var response = await _httpClient.GetAsync($"{_webApiOptions.BaseUrl}/{_webApiOptions.GetProject}/{projectId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProjectResult>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }

        public async Task<Result> UpdateProject(long projectId, ProjectModel model)
        {
            var result = new Result { Successful = true };

            var modelJson = JsonSerializer.Serialize(model);
            var response = await _httpClient.PutAsync($"{_webApiOptions.BaseUrl}/{_webApiOptions.UpdateProject}/{projectId}", new StringContent(modelJson, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                return result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    var badRequest = JsonSerializer.Deserialize<BadRequest>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    result.Successful = false;
                    result.Error = badRequest.Errors.First();

                    break;

                case HttpStatusCode.UnprocessableEntity:
                    var unprocessableEntity = JsonSerializer.Deserialize<UnprocessableEntity>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    result.Successful = false;
                    result.Error = unprocessableEntity.Error;

                    break;

                case HttpStatusCode.Unauthorized:

                    result.Successful = false;
                    result.Error = "UNAUTHORIZED";

                    break;
            }

            return result;
        }
    }
}

