using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class DeleteEnum : IRequest
    {
        public int Id { get; set; }
    }
}
