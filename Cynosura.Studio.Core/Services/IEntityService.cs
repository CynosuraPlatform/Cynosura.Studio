using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;

namespace Cynosura.Studio.Core.Services
{
    public interface IEntityService
    {
		Task<Entity> GetEntityAsync(int solutionId, Guid id);
        Task<PageModel<Entity> > GetEntitiesAsync(int solutionId, int? pageIndex = null, int? pageSize = null);
        Task<Guid> CreateEntityAsync(int solutionId, EntityCreateModel model);
        Task UpdateEntityAsync(int solutionId, Guid id, EntityUpdateModel model);
        Task DeleteEntityAsync(int solutionId, Guid id);
    }
}
