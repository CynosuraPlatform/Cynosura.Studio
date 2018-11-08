using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Entities.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class GetEntities : IRequest<PageModel<EntityModel>>
    {
        public int SolutionId { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
