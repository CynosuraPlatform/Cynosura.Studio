using MediatR;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Views.Models;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class GetViews : IRequest<PageModel<ViewModel>>
    {
        public int SolutionId { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public ViewFilter? Filter { get; set; }
        public string? OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
