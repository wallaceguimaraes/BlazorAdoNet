using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Entities.Projects;
using WebApi.Models.ViewModel.Projects;

namespace WebApi.Services.Projects
{
    public interface IProjectService
    {
        Task<string> AddProject(Project project);
        Task<(List<Project> projects, string error)> GetProjects(FindProjectModel model, long userId);
        Task<(Project project, string error)> GetProjectById(long projectId, long userId);
        Task<string> UpdateProject(Project project);
        Task<string> DeleteProject(long projectId, long userId);

    }
}