using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;

namespace Cynosura.Studio.Core.Services
{
    public class ProjectService : IProjectService
    {
		private readonly IEntityRepository<Project> _projectRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IEntityRepository<Project> projectRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            return await _projectRepository.GetEntities()
                .Where(e => e.Id == id)
				.FirstOrDefaultAsync(_projectRepository);
        }

        public async Task<PageModel<Project> > GetProjectsAsync(int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Project> query = _projectRepository.GetEntities();
            query = query.OrderBy(e => e.Id);
            return await query.ToPagedListAsync(_projectRepository, pageIndex, pageSize);
        }

        public async Task<int> CreateProjectAsync(ProjectCreateModel model)
        {
            var project = _mapper.Map<ProjectCreateModel, Project>(model);
            _projectRepository.Add(project);
            await _unitOfWork.CommitAsync();
            return project.Id;
        }

        public async Task UpdateProjectAsync(int id, ProjectUpdateModel model)
        {
            var project = await GetProjectAsync(id);
            if (project == null)
                return;
            _mapper.Map(model, project);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await GetProjectAsync(id);
            if (project == null)
                return;
            _projectRepository.Delete(project);
            await _unitOfWork.CommitAsync();
        }
    }
}
