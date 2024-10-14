using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.Identity;
using WebApi.Models.ViewModel;
using WebApi.Models.ViewModel.Projects;
using WebApi.Results.Errors;
using WebApi.Services;
using WebApi.Services.Projects;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
    
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectRegisterModel model)
        {
            var userId = HttpContext.User.Claims.UserId();
            
            var error = await _projectService.AddProject(model.Map(userId));

            if (!string.IsNullOrEmpty(error))
                return new UnprocessableEntityJson(error);

            return Ok();
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetUsers([FromBody] FindProjectModel model)
        {
            var userId = HttpContext.User.Claims.UserId();

            var (projects, error) = await _projectService.GetProjects(model, userId);

            if (!string.IsNullOrEmpty(error))
                return new UnprocessableEntityJson(error);

            return Ok(projects);
        }

        [HttpGet, Route("{projectId}") ]
        public async Task<IActionResult> GetProjectById([FromRoute] long projectId)
        {
            var userId = HttpContext.User.Claims.UserId();

            var (project, error) = await _projectService.GetProjectById(projectId, userId);

            if (!string.IsNullOrEmpty(error))
                return new UnprocessableEntityJson(error);

            return Ok(project);
        }

        [HttpPut, Route("update/{projectId}")]
        public async Task<IActionResult> UpdateProject([FromRoute] long projectId, [FromBody] ProjectUpdateModel model)
        {
            var userId = HttpContext.User.Claims.UserId();

            var error = await _projectService.UpdateProject(model.Map(projectId, userId));

            if (!string.IsNullOrEmpty(error))
                return new UnprocessableEntityJson(error);

            return Ok();
        }

        [HttpDelete, Route("delete/{projectId}")]
        public async Task<IActionResult> DeleteProject([FromRoute] long projectId)
        {
            var userId = HttpContext.User.Claims.UserId();

            var error = await _projectService.DeleteProject(projectId, userId);

            if (!string.IsNullOrEmpty(error))
                return new UnprocessableEntityJson(error);

            return NoContent();
        }
    }
}