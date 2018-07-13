using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Web.Infrastructure;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models;
using Cynosura.Studio.Web.Models.ProjectViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [ValidateModel]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<PageModel<ProjectViewModel> > GetProjectsAsync(int? pageIndex, int? pageSize)
        {
            var projects = await _projectService.GetProjectsAsync(pageIndex, pageSize);
            return projects.Map<Project, ProjectViewModel>(_mapper);
        }

        [HttpGet("{id:int}")]
        public async Task<ProjectViewModel> GetProjectAsync(int id)
        {
            var project = await _projectService.GetProjectAsync(id);
            return _mapper.Map<Project, ProjectViewModel>(project);
        }

        [HttpPut("{id:int}")]
        public async Task<StatusViewModel> PutProjectAsync(int id, [FromBody] ProjectUpdateViewModel project)
        {
            var model = _mapper.Map<ProjectUpdateViewModel, ProjectUpdateModel>(project);
            await _projectService.UpdateProjectAsync(id, model);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostProjectAsync([FromBody] ProjectCreateViewModel project)
        {
            var model = _mapper.Map<ProjectCreateViewModel, ProjectCreateModel>(project);
            await _projectService.CreateProjectAsync(model);
            return new StatusViewModel();
        }

        [HttpDelete("{id:int}")]
        public async Task<StatusViewModel> DeleteProjectAsync(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return new StatusViewModel();
        }
    }
}