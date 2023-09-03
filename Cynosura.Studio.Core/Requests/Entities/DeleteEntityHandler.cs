using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Cynosura.Core.Data;
using Cynosura.Core.Services;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class DeleteEntityHandler : IRequestHandler<DeleteEntity>
    {
        private readonly EntityGenerator _entityGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DeleteEntityHandler(EntityGenerator entityGenerator,
            IEntityRepository<Solution> solutionRepository,
            IStringLocalizer<SharedResource> localizer)
        {
            _entityGenerator = entityGenerator;
            _solutionRepository = solutionRepository;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteEntity request, CancellationToken cancellationToken)
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
            if (entity == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Entity"], request.Id]);
            }
            await _entityGenerator.DeleteEntityAsync(solutionAccessor, entity);
            await _entityGenerator.DeleteEntityViewAsync(solutionAccessor, entity);
            await solutionAccessor.DeleteEntityAsync(request.Id);
            return Unit.Value;
        }

    }
}
