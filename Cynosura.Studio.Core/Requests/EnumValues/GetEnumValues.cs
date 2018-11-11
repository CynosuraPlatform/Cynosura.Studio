using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.EnumValues.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class GetEnumValues : IRequest<PageModel<EnumValueModel>>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
