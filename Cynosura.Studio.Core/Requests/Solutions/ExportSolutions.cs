using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Solutions.Models;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class ExportSolutions : IRequest<FileContentModel>
    {
        public SolutionFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
