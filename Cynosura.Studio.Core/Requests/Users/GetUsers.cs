using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Users.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class GetUsers : IRequest<PageModel<UserModel>>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
