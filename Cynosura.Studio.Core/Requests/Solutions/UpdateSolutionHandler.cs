using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Cynosura.Core.Data;
using Cynosura.Core.Services;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class UpdateSolutionHandler : IRequestHandler<UpdateSolution>
    {
        private readonly SolutionGenerator _solutionGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UpdateSolutionHandler(SolutionGenerator solutionGenerator,
            IEntityRepository<Solution> solutionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _solutionGenerator = solutionGenerator;
            _solutionRepository = solutionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(UpdateSolution request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (solution == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Solution"], request.Id]);
            }
            _mapper.Map(request, solution);
            await _unitOfWork.CommitAsync();

            var solutionAccessor = new SolutionAccessor(solution.Path);
            if (solutionAccessor.Metadata.TemplateName != request.TemplateName || solutionAccessor.Metadata.TemplateVersion != request.TemplateVersion)
            {
                await _solutionGenerator.UpgradeSolutionAsync(solutionAccessor, request.TemplateName, request.TemplateVersion);
            }
            return Unit.Value;
        }

    }
}
