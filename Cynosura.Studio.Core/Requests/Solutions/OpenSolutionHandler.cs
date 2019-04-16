using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator;
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
            var solution = _mapper.Map<Solution>(request);
            if (string.IsNullOrEmpty(solution.Name))
            {
                solution.Name = accessor.Metadata.Name;
            }
            _solutionRepository.Add(solution);
            await _unitOfWork.CommitAsync();
            return solution.Id;
        }
    }
}