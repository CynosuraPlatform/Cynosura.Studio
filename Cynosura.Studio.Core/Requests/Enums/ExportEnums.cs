using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Enums.Models;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class ExportEnums : IRequest<FileContentModel>
    {
        public int SolutionId { get; set; }
        public EnumFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
