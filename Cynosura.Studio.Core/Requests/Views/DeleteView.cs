using System;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class DeleteView : IRequest
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }
    }
}
