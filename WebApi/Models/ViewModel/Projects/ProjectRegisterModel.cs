using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApi.Models.Entities.Projects;

namespace WebApi.Models.ViewModel.Projects
{
    public class ProjectRegisterModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("finishedAt")]
        public DateTime? FinishedAt { get; set; }


        public Project Map(long userId) {

            return new Project
            {
                Title = Title,
                FinishedAt = FinishedAt,
                UserId = userId
            };
        }
    }
}