using WebApi.Models.Entities.Projects;
using WebApi.Models.ViewModel.Projects;
using WebApi.Repositories.Projects;
using WebApi.Services.Users;

namespace WebApi.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserService _userService;
        private const string PROJECT_NOT_FOUND = "PROJECT_NOT_FOUND";
        private const string CREATE_PROJECT_ERROR = "CREATE_PROJECT_ERROR";
        private const string LIST_PROJECT_ERROR = "LIST_PROJECT_ERROR";
        private const string GET_PROJECT_ERROR = "GET_PROJECT_ERROR";
        private const string UPDATE_PROJECT_ERROR = "UPDATE_PROJECT_ERROR";
        private const string DELETE_PROJECT_ERROR = "DELETE_PROJECT_ERROR";



        public ProjectService(IProjectRepository projectRepository, IUserService userService)
        {
            _projectRepository = projectRepository;
            _userService = userService;
        }

        public async Task<string> AddProject(Project project)
        {
            try{
                await _projectRepository.AddProject(project);
                return null;
            }catch(Exception){
                //log error
                return CREATE_PROJECT_ERROR;
            }
        }

        public async Task<(List<Project> projects, string? error)> GetProjects(FindProjectModel model, long userId){
            try
            {
                var (user, error) = await _userService.GetUserById(userId);

                if (!string.IsNullOrEmpty(error))
                    return (new List<Project>(), error);

                return (await _projectRepository.GetProjects(model, userId), null);
            }
            catch (Exception)
            {
                //log error
                return (new List<Project>(), LIST_PROJECT_ERROR);
            }
        }

        public async Task<(Project project, string? error)> GetProjectById(long projectId, long userId)
        {
            try{
                return (await _projectRepository.GetProjectByIdAndUser(projectId, userId), null);
            }catch(Exception){
                return (null, GET_PROJECT_ERROR);
            }
        }

        public async Task<string?> UpdateProject(Project project)
        {
            try
            {
                if (_projectRepository.GetProjectById(project.Id) == null)
                    return PROJECT_NOT_FOUND;

                var (user, error) = await _userService.GetUserById(project.UserId);

                if (!string.IsNullOrEmpty(error))
                    return error;

                await _projectRepository.UpdateProject(project);
                return null;
            }
            catch (Exception)
            {
                return UPDATE_PROJECT_ERROR;
            }
        }

        public async Task<string?> DeleteProject(long projectId, long userId)
        {
            try
            {
                var project = _projectRepository.GetProjectById(projectId);

                if(project == null)
                    return PROJECT_NOT_FOUND;

                var (user, error) = await _userService.GetUserById(userId);

                if (!string.IsNullOrEmpty(error))
                    return error;

                await _projectRepository.DeleteProject(projectId, userId);
                return null;
            }
            catch (Exception)
            {
                return DELETE_PROJECT_ERROR;
            }
        }
    }
}