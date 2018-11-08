using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GenerateSolution : IRequest
    {
        public int Id { get; set; }
    }
}
