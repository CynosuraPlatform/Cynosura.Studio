using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Solutions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GetSolutionsHandler : IRequestHandler<GetSolutions, PageModel<SolutionModel>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public GetSolutionsHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<PageModel<SolutionModel>> Handle(GetSolutions request, CancellationToken cancellationToken)
        {
            IQueryable<Solution> query = _solutionRepository.GetEntities()
;
            query = query.OrderBy(e => e.Id);
            var solutions = await query.ToPagedListAsync(request.PageIndex, request.PageSize);
            return solutions.Map<Solution, SolutionModel>(_mapper);
        }

    }
}
