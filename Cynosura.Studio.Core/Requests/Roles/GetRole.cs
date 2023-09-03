using System;
using MediatR;
using Cynosura.Studio.Core.Requests.Roles.Models;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class GetRole : IRequest<RoleModel?>
    {
        public int Id { get; set; }
    }
}
