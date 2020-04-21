using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class UpdateEntityHandler : IRequestHandler<UpdateEntity>
    {
        private readonly EntityGenerator _entityGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public UpdateEntityHandler(EntityGenerator entityGenerator,
            IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _entityGenerator = entityGenerator;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEntity request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var newEntity = _mapper.Map<UpdateEntity, Generator.Models.Entity>(request);
            var oldEntity = (await solutionAccessor.GetEntitiesAsync()).FirstOrDefault(e => e.Id == request.Id);
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
