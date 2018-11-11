using Cynosura.Studio.Core.Requests.EnumValues.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class GetEnumValue : IRequest<EnumValueModel>
    {
        public int Id { get; set; }
    }
}
