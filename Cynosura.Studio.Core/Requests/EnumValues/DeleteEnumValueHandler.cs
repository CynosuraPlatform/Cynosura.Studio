using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class DeleteEnumValueHandler : IRequestHandler<DeleteEnumValue>
    {
        private readonly IEntityRepository<EnumValue> _enumValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEnumValueHandler(IEntityRepository<EnumValue> enumValueRepository,
            IUnitOfWork unitOfWork)
        {
            _enumValueRepository = enumValueRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteEnumValue request, CancellationToken cancellationToken)
        {
            var enumValue = await _enumValueRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (enumValue != null)
            {
                _enumValueRepository.Delete(enumValue);
                await _unitOfWork.CommitAsync();
            }
            return Unit.Value;
        }

    }
}
