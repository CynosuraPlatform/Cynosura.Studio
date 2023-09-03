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
using Cynosura.Studio.Generator;
using EnumModel = Cynosura.Studio.Core.Requests.Enums.Models.EnumModel;
using Cynosura.Core.Services;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnumsHandler : IRequestHandler<GetEnums, PageModel<EnumModel>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GetEnumsHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PageModel<EnumModel>> Handle(GetEnums request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            if (solution == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Solution"], request.SolutionId]);
            }
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var enums = await solutionAccessor.GetEnumsAsync();
            if (!string.IsNullOrEmpty(request.Filter?.Text))
            {
                enums = enums.Where(e => e.Name.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase) || 
                                         e.DisplayName.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            return enums.ToPagedList(request.PageIndex, request.PageSize)
                .Map<Generator.Models.Enum, EnumModel>(_mapper);
        }

    }
}
