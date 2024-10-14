using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApi.Models.Entities.Projects;

namespace WebApi.Models.ViewModel.Projects
{
    public class ProjectUpdateModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("finishedAt")]
        public DateTime? FinishedAt { get; set; }


        public Project Map(long projectId, long userId)
        {
            return new Project
            {
                Id = projectId,
                Title = Title,
                CreatedAt = CreatedAt.Value,
                FinishedAt = FinishedAt,
                UserId = userId
            };
        }
    }
}