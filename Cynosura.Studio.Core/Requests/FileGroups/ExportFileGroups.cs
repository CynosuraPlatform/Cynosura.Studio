using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.FileGroups.Models;

namespace Cynosura.Studio.Core.Requests.FileGroups
{
    public class ExportFileGroups : IRequest<FileContentModel>
    {
        public FileGroupFilter? Filter { get; set; }
        public string? OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
