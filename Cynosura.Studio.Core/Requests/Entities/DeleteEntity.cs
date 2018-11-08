using System;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class DeleteEntity : IRequest
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }
    }
}
