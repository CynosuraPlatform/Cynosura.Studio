using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Enums.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnums : IRequest<PageModel<EnumModel>>
    {
        public int SolutionId { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public EnumFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
