using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;

namespace Cynosura.Studio.Core.Services
{
    public interface ISolutionService
    {
        Task<Solution> GetSolutionAsync(int id);
        Task<PageModel<Solution> > GetSolutionsAsync(int? pageIndex = null, int? pageSize = null);
        Task<int> CreateSolutionAsync(SolutionCreateModel model);
        Task UpdateSolutionAsync(int id, SolutionUpdateModel model);
        Task DeleteSolutionAsync(int id);
        Task GenerateAsync(int id);
        Task UpgradeAsync(int id);
    }
}
