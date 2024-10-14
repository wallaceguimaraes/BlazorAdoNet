using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApi.Models.ViewModel.Projects
{
    public class FindProjectModel
    {
        [JsonProperty("title")]
        public string? Title { get; set; }
        
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }
    }
}