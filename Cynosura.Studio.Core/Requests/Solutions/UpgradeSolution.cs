using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class UpgradeSolution : IRequest
    {
        public int Id { get; set; }
    }
}
