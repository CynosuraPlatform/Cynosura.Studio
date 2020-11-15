using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Files.Models;

namespace Cynosura.Studio.Core.Requests.Files
{
    public class ExportFiles : IRequest<FileContentModel>
    {
        public FileFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
