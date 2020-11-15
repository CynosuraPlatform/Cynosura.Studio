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

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class UpdateEnumHandler : IRequestHandler<UpdateEnum>
    {
        private readonly EnumGenerator _enumGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UpdateEnumHandler(EnumGenerator enumGenerator,
            IEntityRepository<Solution> solutionRepository,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _enumGenerator = enumGenerator;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(UpdateEnum request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var newEnum = _mapper.Map<UpdateEnum, Generator.Models.Enum>(request);
            var oldEnum = (await solutionAccessor.GetEnumsAsync()).FirstOrDefault(e => e.Id == request.Id);
            if (oldEnum == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Enum"], request.Id]);
            }
            await solutionAccessor.UpdateEnumAsync(newEnum);
            // reload Enum from Solution
            newEnum = (await solutionAccessor.GetEnumsAsync())
                .FirstOrDefault(e => e.Id == request.Id);
            await _enumGenerator.UpgradeEnumAsync(solutionAccessor, oldEnum, newEnum);
            await _enumGenerator.UpgradeEnumViewAsync(solutionAccessor, oldEnum, newEnum);
            return Unit.Value;
        }

    }
}
