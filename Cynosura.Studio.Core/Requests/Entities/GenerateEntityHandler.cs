using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class GenerateEntityHandler : IRequestHandler<GenerateEntity>
    {
        private readonly EntityGenerator _entityGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;

        public GenerateEntityHandler(EntityGenerator entityGenerator,
            IEntityRepository<Solution> solutionRepository)
        {
            _entityGenerator = entityGenerator;
            _solutionRepository = solutionRepository;
        }

        public async Task<Unit> Handle(GenerateEntity request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var entity = (await solutionAccessor.GetEntitiesAsync()).FirstOrDefault(e => e.Id == request.Id);
            await _entityGenerator.GenerateEntityAsync(solutionAccessor, entity);
            await _entityGenerator.GenerateEntityViewAsync(solutionAccessor, entity);
            return Unit.Value;
        }

    }
}
