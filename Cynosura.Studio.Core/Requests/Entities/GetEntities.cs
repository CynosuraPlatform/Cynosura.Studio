using MediatR;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Entities.Models;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class GetEntities : IRequest<PageModel<EntityModel>>
    {
        public int SolutionId { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public Models.EntityFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
