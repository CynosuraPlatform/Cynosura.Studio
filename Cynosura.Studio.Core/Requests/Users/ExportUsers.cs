using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Users.Models;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class ExportUsers : IRequest<FileContentModel>
    {
        public UserFilter? Filter { get; set; }
        public string? OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
