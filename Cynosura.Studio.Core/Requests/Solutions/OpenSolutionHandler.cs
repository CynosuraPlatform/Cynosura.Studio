using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using Cynosura.Studio.Core.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class OpenSolutionHandler : IRequestHandler<OpenSolution, int>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OpenSolutionHandler(IEntityRepository<Solution> solutionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(OpenSolution request, CancellationToken cancellationToken)
        {
            var accessor = new SolutionAccessor(request.Path);
            var solution = new Solution(accessor.Metadata.Name, accessor.Path);
            _solutionRepository.Add(solution);
            await _unitOfWork.CommitAsync();
            return solution.Id;
        }
    }
}
