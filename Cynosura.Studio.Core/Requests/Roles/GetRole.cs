using Cynosura.Studio.Core.Requests.Roles.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class GetRole : IRequest<RoleModel>
    {
        public int Id { get; set; }
    }
}
