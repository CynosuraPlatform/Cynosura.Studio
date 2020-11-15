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

namespace Cynosura.Studio.Core.Requests.Views
{
    public class CreateViewHandler : IRequestHandler<CreateView, CreatedEntity<Guid>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly ViewGenerator _viewGenerator;
        private readonly IMapper _mapper;

        public CreateViewHandler(IEntityRepository<Solution> solutionRepository,
            ViewGenerator viewGenerator,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _viewGenerator = viewGenerator;
            _mapper = mapper;
        }

        public async Task<CreatedEntity<Guid>> Handle(CreateView request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var view = _mapper.Map<CreateView, Generator.Models.View>(request);
            view.Id = Guid.NewGuid();
            await solutionAccessor.CreateViewAsync(view);

            await _viewGenerator.GenerateViewAsync(solutionAccessor, view);
            return new CreatedEntity<Guid>() { Id = view.Id };
        }

    }
}
