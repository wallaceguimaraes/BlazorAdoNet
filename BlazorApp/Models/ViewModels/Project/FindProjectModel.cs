using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Models.ViewModels.Project
{
    public class FindProjectModel
    {
        public string Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CurrentPage { get; set; } = 1;
    }
}