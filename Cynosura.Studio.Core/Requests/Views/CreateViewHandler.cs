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
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class CreateViewHandler : IRequestHandler<CreateView, CreatedEntity<Guid>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public CreateViewHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
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
            return new CreatedEntity<Guid>() { Id = view.Id };
        }

    }
}
