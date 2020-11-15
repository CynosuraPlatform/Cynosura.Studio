using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Entities.Models;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class ExportEntities : IRequest<FileContentModel>
    {
        public int SolutionId { get; set; }
        public Models.EntityFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
