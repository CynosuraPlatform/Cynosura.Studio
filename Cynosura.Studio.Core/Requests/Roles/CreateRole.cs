using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class CreateRole : IRequest<int>
    {
        public string Name { get; set; }
    }
}
