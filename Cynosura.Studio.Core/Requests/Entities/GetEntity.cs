using System;
using Cynosura.Studio.Core.Requests.Entities.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class GetEntity : IRequest<EntityModel>
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }
    }
}
