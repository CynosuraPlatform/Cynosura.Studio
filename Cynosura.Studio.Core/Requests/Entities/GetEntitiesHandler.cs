using System;
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
            if (!string.IsNullOrEmpty(request.Filter?.Text))
            {
                entities = entities.Where(e => e.Name.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                               e.PluralName.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                               e.DisplayName.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                               e.PluralDisplayName.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            return entities.ToPagedList(request.PageIndex, request.PageSize)
                .Map<Generator.Models.Entity, EntityModel>(_mapper);
        }

    }
}
