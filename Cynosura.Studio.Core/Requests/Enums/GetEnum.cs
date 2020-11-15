using System;
using MediatR;
using Cynosura.Studio.Core.Requests.Enums.Models;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnum : IRequest<EnumModel>
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }
    }
}
