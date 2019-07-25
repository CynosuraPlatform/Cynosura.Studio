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
        private readonly ITemplateProvider _templateProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSolutionHandler(CodeGenerator codeGenerator,
            IEntityRepository<Solution> solutionRepository,
            ITemplateProvider templateProvider,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _codeGenerator = codeGenerator;
            _solutionRepository = solutionRepository;
            _templateProvider = templateProvider;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreatedEntity<int>> Handle(CreateSolution request, CancellationToken cancellationToken)
        {
            var solution = _mapper.Map<CreateSolution, Solution>(request);
            var template = await _templateProvider.GetTemplateAsync(request.TemplateName);
            if(template == null)
                throw new ArgumentException($"Template '{request.TemplateName}' not found");
            _solutionRepository.Add(solution);
            await _unitOfWork.CommitAsync();
            await _codeGenerator.GenerateSolutionAsync(solution.Path, solution.Name, template.Name);
            return new CreatedEntity<int>() { Id = solution.Id };
        }

    }
}
