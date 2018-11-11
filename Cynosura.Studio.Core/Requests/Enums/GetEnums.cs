using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Enums.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnums : IRequest<PageModel<EnumModel>>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
