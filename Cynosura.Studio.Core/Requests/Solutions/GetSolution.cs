using Cynosura.Studio.Core.Requests.Solutions.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GetSolution : IRequest<SolutionModel>
    {
        public int Id { get; set; }
    }
}
