using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Core.Services;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class GenerateEntityHandler : IRequestHandler<GenerateEntity>
    {
        private readonly EntityGenerator _entityGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GenerateEntityHandler(EntityGenerator entityGenerator,
            IEntityRepository<Solution> solutionRepository,
            IStringLocalizer<SharedResource> localizer)
        {
            _entityGenerator = entityGenerator;
            _solutionRepository = solutionRepository;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(GenerateEntity request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            if (solution == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Solution"], request.SolutionId]);
            }
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var entity = (await solutionAccessor.GetEntitiesAsync()).FirstOrDefault(e => e.Id == request.Id);
            await _entityGenerator.GenerateEntityAsync(solutionAccessor, entity);
            await _entityGenerator.GenerateEntityViewAsync(solutionAccessor, entity);
            return Unit.Value;
        }

    }
}
