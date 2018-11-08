using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Solutions.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GetSolutions : IRequest<PageModel<SolutionModel>>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
