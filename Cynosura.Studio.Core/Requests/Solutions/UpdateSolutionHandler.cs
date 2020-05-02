using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class UpdateSolutionHandler : IRequestHandler<UpdateSolution>
    {
        private readonly SolutionGenerator _solutionGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSolutionHandler(SolutionGenerator solutionGenerator,
            IEntityRepository<Solution> solutionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _solutionGenerator = solutionGenerator;
            _solutionRepository = solutionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateSolution request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (solution != null)
            {
                _mapper.Map(request, solution);
                await _unitOfWork.CommitAsync();

                var solutionAccessor = new SolutionAccessor(solution.Path);
                if (solutionAccessor.Metadata.TemplateName != request.TemplateName || solutionAccessor.Metadata.TemplateVersion != request.TemplateVersion)
                {
                    await _solutionGenerator.UpgradeSolutionAsync(solutionAccessor, request.TemplateName, request.TemplateVersion);
                }
            }
            return Unit.Value;
        }

    }
}
