using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Models.Results.Errors
{
    public class BadRequest
    {
        public ICollection<string> Errors { get; set; }

    }
}