using System;
using MediatR;
using Cynosura.Studio.Core.Requests.Solutions.Models;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GetSolution : IRequest<SolutionModel>
    {
        public int Id { get; set; }
    }
}
