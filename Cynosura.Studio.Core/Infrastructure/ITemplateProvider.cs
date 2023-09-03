using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.Infrastructure
{
    public interface ITemplateProvider
    {
        Task<IEnumerable<TemplateModel>> GetTemplatesAsync();
        Task<TemplateModel?> GetTemplateAsync(string name);
    }
}
