using System;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class DeleteEnum : IRequest
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }
    }
}
