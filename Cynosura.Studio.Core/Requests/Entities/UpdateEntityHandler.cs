using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Cynosura.Core.Data;
using Cynosura.Core.Services;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class UpdateEntityHandler : IRequestHandler<UpdateEntity>
    {
        private readonly EntityGenerator _entityGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UpdateEntityHandler(EntityGenerator entityGenerator,
            IEntityRepository<Solution> solutionRepository,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _entityGenerator = entityGenerator;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(UpdateEntity request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var newEntity = _mapper.Map<UpdateEntity, Generator.Models.Entity>(request);
            var oldEntity = (await solutionAccessor.GetEntitiesAsync()).FirstOrDefault(e => e.Id == request.Id);
            if (oldEntity == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Entity"], request.Id]);
            }
            await solutionAccessor.UpdateEntityAsync(newEntity);
            // reload Entity from Solution
            newEntity = (await solutionAccessor.GetEntitiesAsync())
                .FirstOrDefault(e => e.Id == request.Id);
            await _entityGenerator.UpgradeEntityAsync(solutionAccessor, oldEntity, newEntity);
            await _entityGenerator.UpgradeEntityViewAsync(solutionAccessor, oldEntity, newEntity);
            return Unit.Value;
        }

    }
}
