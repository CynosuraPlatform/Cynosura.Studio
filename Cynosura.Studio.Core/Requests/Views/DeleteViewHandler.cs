using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class DeleteViewHandler : IRequestHandler<DeleteView>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly ViewGenerator _viewGenerator;

        public DeleteViewHandler(IEntityRepository<Solution> solutionRepository,
            ViewGenerator viewGenerator)
        {
            _solutionRepository = solutionRepository;
            _viewGenerator = viewGenerator;
        }

        public async Task<Unit> Handle(DeleteView request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var view = (await solutionAccessor.GetViewsAsync()).FirstOrDefault(e => e.Id == request.Id);
            await _viewGenerator.DeleteViewAsync(solutionAccessor, view);
            await solutionAccessor.DeleteViewAsync(request.Id);
            return Unit.Value;
        }

    }
}
