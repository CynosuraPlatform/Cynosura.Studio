using MediatR;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class DeleteEnumValue : IRequest
    {
        public int Id { get; set; }
    }
}
