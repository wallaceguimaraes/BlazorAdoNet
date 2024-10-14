using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient();
            
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions();
        
        }
        }
}