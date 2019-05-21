using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class DeleteEnumHandler : IRequestHandler<DeleteEnum>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;

        public DeleteEnumHandler(IEntityRepository<Solution> solutionRepository)
        {
            _solutionRepository = solutionRepository;
        }

        public async Task<Unit> Handle(DeleteEnum request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            await solutionAccessor.DeleteEnumAsync(request.Id);
            return Unit.Value;
        }

    }
}
