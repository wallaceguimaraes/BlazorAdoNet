
using BlazorApp.Models.Results;
using BlazorApp.Models.Results.Project;
using BlazorApp.Models.ViewModels.Project;

namespace BlazorApp.Services.Project
{
    public interface IProjectService
    {
        Task<List<ProjectResult>> GetProjects(FindProjectModel model);
        Task<Result> DeleteProject(long projectId);
        Task<Result> AddProject(ProjectModel model);
        Task<ProjectResult> GetProjectById(long projectId);
        Task<Result> UpdateProject(long projectId, ProjectModel model);

    }

}