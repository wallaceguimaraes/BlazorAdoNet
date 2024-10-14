using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Entities.Users;

namespace WebApi.Models.Entities.Projects
{
    public class Project
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }

    }
}