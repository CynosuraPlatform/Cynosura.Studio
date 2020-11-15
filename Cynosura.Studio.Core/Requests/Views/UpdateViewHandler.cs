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

namespace Cynosura.Studio.Core.Requests.Views
{
    public class UpdateViewHandler : IRequestHandler<UpdateView>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UpdateViewHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(UpdateView request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var newView = _mapper.Map<UpdateView, Generator.Models.View>(request);
            var oldView = (await solutionAccessor.GetViewsAsync()).FirstOrDefault(e => e.Id == request.Id);
            if (oldView == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["View"], request.Id]);
            }
            await solutionAccessor.UpdateViewAsync(newView);
            return Unit.Value;
        }

    }
}
