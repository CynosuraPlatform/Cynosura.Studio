using MediatR;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class DeleteRole : IRequest
    {
        public int Id { get; set; }
    }
}
