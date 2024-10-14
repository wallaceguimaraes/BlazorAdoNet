

using BlazorApp.Models.IntegrationModel;
using BlazorApp.Providers;
using BlazorApp.Services.Auth;
using BlazorApp.Services.Project;
using BlazorApp.Services.User;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ApiAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<ApiAuthenticationStateProvider>());
            services.AddBlazoredLocalStorage();

        }

        public static void AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions();
            services.Configure<WebApiOptions>(config.GetSection("WebApi"));
        }

        
    }
}