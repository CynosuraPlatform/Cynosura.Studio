using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GenerateEnumHandler : IRequestHandler<GenerateEnum>
    {
        private readonly EnumGenerator _enumGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;

        public GenerateEnumHandler(EnumGenerator enumGenerator,
            IEntityRepository<Solution> solutionRepository)
        {
            _enumGenerator = enumGenerator;
            _solutionRepository = solutionRepository;
        }

        public async Task<Unit> Handle(GenerateEnum request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var @enum = (await solutionAccessor.GetEnumsAsync()).FirstOrDefault(e => e.Id == request.Id);
            await _enumGenerator.GenerateEnumAsync(solutionAccessor, @enum);
            await _enumGenerator.GenerateEnumViewAsync(solutionAccessor, @enum);
            return Unit.Value;
        }

    }
}
