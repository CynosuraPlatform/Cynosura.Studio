using MediatR;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Files.Models;

namespace Cynosura.Studio.Core.Requests.Files
{
    public class GetFiles : IRequest<PageModel<FileModel>>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public FileFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
