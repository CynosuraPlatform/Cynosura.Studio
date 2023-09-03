using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Core.Services;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GenerateEnumHandler : IRequestHandler<GenerateEnum>
    {
        private readonly EnumGenerator _enumGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GenerateEnumHandler(EnumGenerator enumGenerator,
            IEntityRepository<Solution> solutionRepository,
            IStringLocalizer<SharedResource> localizer)
        {
            _enumGenerator = enumGenerator;
            _solutionRepository = solutionRepository;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(GenerateEnum request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            if (solution == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Solution"], request.SolutionId]);
            }
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var @enum = (await solutionAccessor.GetEnumsAsync()).FirstOrDefault(e => e.Id == request.Id);
            await _enumGenerator.GenerateEnumAsync(solutionAccessor, @enum);
            await _enumGenerator.GenerateEnumViewAsync(solutionAccessor, @enum);
            return Unit.Value;
        }

    }
}
