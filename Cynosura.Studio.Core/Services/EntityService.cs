using System;
using System.Collections.Generic;
using System.IO;
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
    public class EntityService : IEntityService
    {
        private readonly IProjectService _projectService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntityService(IProjectService projectService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _projectService = projectService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Entity> GetEntityAsync(int projectId, Guid id)
        {
            var entities = await GetEntitiesAsync(projectId);
            return entities.PageItems
                .FirstOrDefault(e => e.Id == id);
        }

        private List<Entity> GetEntitiesFromSolution(string path)
        {
            var solution = new Solution.Solution(path);
            return solution.GetEntities();
        }

        private static PageModel<T> ToPageModel<T>(IList<T> list, int? pageIndex = null, int? pageSize = null)
        {
            if (pageIndex != null && pageSize != null)
            {
                var pageList = list.Skip(pageIndex.Value * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();
                return new PageModel<T>(pageList, list.Count, pageIndex.Value);
            }
            else
            {
                return new PageModel<T>(list, list.Count, 0);
            }
        }

        public async Task<PageModel<Entity> > GetEntitiesAsync(int projectId, int? pageIndex = null, int? pageSize = null)
        {
            var project = await _projectService.GetProjectAsync(projectId);
            var entities = GetEntitiesFromSolution(project.Path);
            return ToPageModel(entities, pageIndex, pageSize);
        }

        public async Task<Guid> CreateEntityAsync(int projectId, EntityCreateModel model)
        {
            return Guid.NewGuid();
        }

        public async Task UpdateEntityAsync(int projectId, Guid id, EntityUpdateModel model)
        {
            
        }

        public async Task DeleteEntityAsync(int projectId, Guid id)
        {
            
        }
    }
}
