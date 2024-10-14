using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using WebApi.Data;
using WebApi.Extensions;
using WebApi.Extensions.Filters;
using WebApi.Infrastructure.Mvc;
using WebApi.Repositories.Projects;
using WebApi.Repositories.Users;
using WebApi.Services.Auth;
using WebApi.Services.Projects;
using WebApi.Services.Users;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(ErrorHandlerAttribute));
                options.Filters.Add(typeof(ModelValidationAttribute));
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new CustomEnumConverter());
            })
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
            services.AddHttpContextAccessor();
            services.AddSwaggerGen();

            services.AddTransient<IDatabaseConection, SqlDatabaseConnection>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProjectService, ProjectService>();

            services.AddTransient<IUserRepository>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MySqlConnection"); // Ou outra chave
                return new UserRepository(connectionString);
            });

            services.AddTransient<IProjectRepository>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MySqlConnection"); // Ou outra chave
                return new ProjectRepository(connectionString);
            });

            services.AddJwtAuthentication(options =>
            {
                Configuration.GetSection("Authorization").Bind(options);
            });

            services.AddControllersWithViews(options =>
                options.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>());

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(policy => policy
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());
            
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseHttpsRedirection();
        }
    }
}
