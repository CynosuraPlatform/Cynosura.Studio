using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class DeleteEntityHandler : IRequestHandler<DeleteEntity>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;

        public DeleteEntityHandler(IEntityRepository<Solution> solutionRepository)
        {
            _solutionRepository = solutionRepository;
        }

        public async Task<Unit> Handle(DeleteEntity request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            await solutionAccessor.DeleteEntityAsync(request.Id);
            return Unit.Value;
        }

    }
}
