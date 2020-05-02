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
    public class UpgradeSolutionHandler : IRequestHandler<UpgradeSolution>
    {
        private readonly SolutionGenerator _solutionGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;

        public UpgradeSolutionHandler(SolutionGenerator solutionGenerator, 
            IEntityRepository<Solution> solutionRepository)
        {
            _solutionGenerator = solutionGenerator;
            _solutionRepository = solutionRepository;
        }

        public async Task<Unit> Handle(UpgradeSolution request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (solution != null)
            {
                await _solutionGenerator.UpgradeSolutionAsync(new SolutionAccessor(solution.Path));
            }
            return Unit.Value;
        }

    }
}
