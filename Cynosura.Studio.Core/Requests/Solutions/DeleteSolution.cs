using System;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class DeleteSolution : IRequest
    {
        public int Id { get; set; }
    }
}
