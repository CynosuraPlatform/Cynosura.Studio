using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EntityModel = Cynosura.Studio.Core.Requests.Entities.Models.EntityModel;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class GetEntityHandler : IRequestHandler<GetEntity, EntityModel>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public GetEntityHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<EntityModel> Handle(GetEntity request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var entities = await solutionAccessor.GetEntitiesAsync();
            var entity = entities.FirstOrDefault(e => e.Id == request.Id);
            return _mapper.Map<Generator.Models.Entity, EntityModel>(entity);
        }

    }
}
