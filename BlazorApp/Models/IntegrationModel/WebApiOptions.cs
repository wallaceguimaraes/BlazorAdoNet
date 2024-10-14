using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Models.IntegrationModel
{
    public class WebApiOptions
    {
        public string BaseUrl { get; set; }
        public string Authenticate { get; set; }
        public string RegisterUser { get; set; }
        public string CreateProject { get; set; }
        public string Logout { get; set; }
        public string DeleteProject { get; set; }
        public string ListProjects { get; set; }
        public string GetProject { get; set; }
        public string UpdateProject { get; set; }
        public string CreateUser { get; set; }
    }
}