using System;
using MediatR;
using Cynosura.Studio.Core.Requests.Entities.Models;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class GetEntity : IRequest<EntityModel>
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }
    }
}
