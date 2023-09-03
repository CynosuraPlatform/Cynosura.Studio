using System;
using MediatR;
using Cynosura.Studio.Core.Requests.Views.Models;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class GetView : IRequest<ViewModel?>
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }
    }
}
