using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Views.Models;
using Cynosura.Studio.Generator;
using Cynosura.Core.Services;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class GetViewHandler : IRequestHandler<GetView, ViewModel?>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GetViewHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<ViewModel?> Handle(GetView request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            if (solution == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Solution"], request.SolutionId]);
            }
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var views = await solutionAccessor.GetViewsAsync();
            var view = views.FirstOrDefault(e => e.Id == request.Id);
            if (view == null)
            {
                return null;
            }
            return _mapper.Map<Generator.Models.View, ViewModel>(view);
        }

    }
}
