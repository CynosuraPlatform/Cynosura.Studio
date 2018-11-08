using Cynosura.Studio.Core.Requests.Users.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class GetUser : IRequest<UserModel>
    {
        public int Id { get; set; }
    }
}
