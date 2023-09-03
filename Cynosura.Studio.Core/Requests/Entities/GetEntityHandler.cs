using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using EntityModel = Cynosura.Studio.Core.Requests.Entities.Models.EntityModel;
using Cynosura.Core.Services;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class GetEntityHandler : IRequestHandler<GetEntity, EntityModel?>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GetEntityHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<EntityModel?> Handle(GetEntity request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            if (solution == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Solution"], request.SolutionId]);
            }
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var entities = await solutionAccessor.GetEntitiesAsync();
            var entity = entities.FirstOrDefault(e => e.Id == request.Id);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<Generator.Models.Entity, EntityModel>(entity);
        }

    }
}
