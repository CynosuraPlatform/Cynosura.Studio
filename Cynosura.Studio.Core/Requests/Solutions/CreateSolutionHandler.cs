using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Generator;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class CreateSolutionHandler : IRequestHandler<CreateSolution, CreatedEntity<int>>
    {
        private readonly CodeGenerator _codeGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSolutionHandler(CodeGenerator codeGenerator,
            IEntityRepository<Solution> solutionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _codeGenerator = codeGenerator;
            _solutionRepository = solutionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreatedEntity<int>> Handle(CreateSolution request, CancellationToken cancellationToken)
        {
            var solution = _mapper.Map<CreateSolution, Solution>(request);
            _solutionRepository.Add(solution);
            await _unitOfWork.CommitAsync();
            await _codeGenerator.GenerateSolutionAsync(solution.Path, solution.Name);
            return new CreatedEntity<int>() { Id = solution.Id };
        }

    }
}
