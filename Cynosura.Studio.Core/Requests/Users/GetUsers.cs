using MediatR;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Users.Models;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class GetUsers : IRequest<PageModel<UserModel>>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public UserFilter? Filter { get; set; }
        public string? OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
