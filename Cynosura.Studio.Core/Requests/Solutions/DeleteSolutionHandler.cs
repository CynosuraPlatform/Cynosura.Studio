using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class DeleteSolutionHandler : IRequestHandler<DeleteSolution>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSolutionHandler(IEntityRepository<Solution> solutionRepository,
            IUnitOfWork unitOfWork)
        {
            _solutionRepository = solutionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteSolution request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (solution != null)
            {
                _solutionRepository.Delete(solution);
                await _unitOfWork.CommitAsync();
            }
            return Unit.Value;
        }

    }
}
