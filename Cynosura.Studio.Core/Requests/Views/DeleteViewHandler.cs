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
        private readonly IUnitOfWork _unitOfWork;

        public DeleteViewHandler(IEntityRepository<Solution> solutionRepository,
            IUnitOfWork unitOfWork)
        {
            _solutionRepository = solutionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteView request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            await solutionAccessor.DeleteViewAsync(request.Id);
            return Unit.Value;
        }

    }
}
