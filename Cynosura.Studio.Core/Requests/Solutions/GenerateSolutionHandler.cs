using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GenerateSolutionHandler : IRequestHandler<GenerateSolution>
    {
        private readonly SolutionGenerator _solutionGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;

        public GenerateSolutionHandler(SolutionGenerator solutionGenerator, 
            IEntityRepository<Solution> solutionRepository)
        {
            _solutionGenerator = solutionGenerator;
            _solutionRepository = solutionRepository;
        }

        public async Task<Unit> Handle(GenerateSolution request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (solution != null)
            {
                var accessor = new SolutionAccessor(solution.Path);
                await _solutionGenerator.GenerateSolutionAsync(solution.Path, solution.Name, accessor.Metadata.TemplateName);
            }
            return Unit.Value;
        }

    }
}
