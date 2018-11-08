using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EntityModel = Cynosura.Studio.Core.Requests.Entities.Models.EntityModel;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class GetEntitiesHandler : IRequestHandler<GetEntities, PageModel<EntityModel>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public GetEntitiesHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<PageModel<EntityModel>> Handle(GetEntities request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var entities = await solutionAccessor.GetEntitiesAsync();
            return entities.ToPagedList(request.PageIndex, request.PageSize)
                .Map<Generator.Models.Entity, EntityModel>(_mapper);
        }

    }
}
