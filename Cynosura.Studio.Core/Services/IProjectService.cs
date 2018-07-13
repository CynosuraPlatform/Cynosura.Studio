using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;

namespace Cynosura.Studio.Core.Services
{
    public interface IProjectService
    {
		Task<Project> GetProjectAsync(int id);
        Task<PageModel<Project> > GetProjectsAsync(int? pageIndex = null, int? pageSize = null);
        Task<int> CreateProjectAsync(ProjectCreateModel model);
        Task UpdateProjectAsync(int id, ProjectUpdateModel model);
        Task DeleteProjectAsync(int id);
    }
}
