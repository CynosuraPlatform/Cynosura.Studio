using MediatR;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class DeleteUser : IRequest
    {
        public int Id { get; set; }
    }
}
