using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Solutions.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GetSolutions : IRequest<PageModel<SolutionModel>>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public SolutionFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
