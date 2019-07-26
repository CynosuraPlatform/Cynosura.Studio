using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Solutions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GetSolutionHandler : IRequestHandler<GetSolution, SolutionModel>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public GetSolutionHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<SolutionModel> Handle(GetSolution request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            var solutionModel = _mapper.Map<Solution, SolutionModel>(solution);
            if (solutionModel != null)
            {
                solutionModel.LoadMetadata();
            }
            return solutionModel;
        }

    }
}
