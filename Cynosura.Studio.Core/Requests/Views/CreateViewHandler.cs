using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Generator;
using Cynosura.Core.Services;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class CreateViewHandler : IRequestHandler<CreateView, CreatedEntity<Guid>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly ViewGenerator _viewGenerator;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CreateViewHandler(IEntityRepository<Solution> solutionRepository,
            ViewGenerator viewGenerator,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _solutionRepository = solutionRepository;
            _viewGenerator = viewGenerator;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<CreatedEntity<Guid>> Handle(CreateView request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            if (solution == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Solution"], request.SolutionId]);
            }
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var view = _mapper.Map<CreateView, Generator.Models.View>(request);
            view.Id = Guid.NewGuid();
            await solutionAccessor.CreateViewAsync(view);

            await _viewGenerator.GenerateViewAsync(solutionAccessor, view);
            return new CreatedEntity<Guid>(view.Id);
        }

    }
}
