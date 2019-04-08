using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator;
using Cynosura.Studio.Core.Generator.Models;
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
            await _codeGenerator.GenerateAsync(solutionAccessor, @enum, new EnumModel(@enum, solutionAccessor), TemplateType.Enum);
            await _codeGenerator.GenerateAsync(solutionAccessor, @enum,
                new EnumViewModel(new View(), @enum, solutionAccessor), TemplateType.EnumView);
            return Unit.Value;
        }

    }
}
