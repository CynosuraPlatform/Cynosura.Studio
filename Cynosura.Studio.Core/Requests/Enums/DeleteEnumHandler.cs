using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class DeleteEnumHandler : IRequestHandler<DeleteEnum>
    {
        private readonly IEntityRepository<Enum> _enumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEnumHandler(IEntityRepository<Enum> enumRepository,
            IUnitOfWork unitOfWork)
        {
            _enumRepository = enumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteEnum request, CancellationToken cancellationToken)
        {
            var @enum = await _enumRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (@enum != null)
            {
                _enumRepository.Delete(@enum);
                await _unitOfWork.CommitAsync();
            }
            return Unit.Value;
        }

    }
}
