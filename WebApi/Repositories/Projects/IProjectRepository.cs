using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Entities.Projects;
using WebApi.Models.ViewModel.Projects;

namespace WebApi.Repositories.Projects
{
    public interface IProjectRepository
    {
        Task AddProject(Project project);
        Task<List<Project>> GetProjects(FindProjectModel model, long userId);
        Task<Project> GetProjectByIdAndUser(long projectId, long userId);
        Task<Project> GetProjectById(long projectId);
        Task UpdateProject(Project project);
        Task DeleteProject(long projectId, long userId);
    }
}