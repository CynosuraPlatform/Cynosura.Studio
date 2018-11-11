using Cynosura.Studio.Core.Requests.Enums.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnum : IRequest<EnumModel>
    {
        public int Id { get; set; }
    }
}
