using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Views.Models;
using Cynosura.Studio.Generator;
using Cynosura.Core.Services;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class GetViewsHandler : IRequestHandler<GetViews, PageModel<ViewModel>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GetViewsHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PageModel<ViewModel>> Handle(GetViews request, CancellationToken cancellationToken)
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
            if (!string.IsNullOrEmpty(request.Filter?.Text))
            {
                views = views.Where(e => e.Name.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            return views.ToPagedList(request.PageIndex, request.PageSize)
                .Map<Generator.Models.View, ViewModel>(_mapper);
        }

    }
}
