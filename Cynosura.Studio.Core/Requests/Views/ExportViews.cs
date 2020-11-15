using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Views.Models;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class ExportViews : IRequest<FileContentModel>
    {
        public int SolutionId { get; set; }
        public ViewFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
