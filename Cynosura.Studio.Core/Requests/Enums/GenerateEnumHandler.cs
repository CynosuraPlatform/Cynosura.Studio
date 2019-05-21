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
        private readonly CodeGenerator _codeGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;

        public GenerateEnumHandler(CodeGenerator codeGenerator,
            IEntityRepository<Solution> solutionRepository)
        {
            _codeGenerator = codeGenerator;
            _solutionRepository = solutionRepository;
        }

        public async Task<Unit> Handle(GenerateEnum request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var @enum = (await solutionAccessor.GetEnumsAsync()).FirstOrDefault(e => e.Id == request.Id);
            await _codeGenerator.GenerateEnumAsync(solutionAccessor, @enum);
            await _codeGenerator.GenerateEnumViewAsync(solutionAccessor, new Generator.Models.View(), @enum);
            return Unit.Value;
        }

    }
}
