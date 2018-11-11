using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class UpdateEnumValueHandler : IRequestHandler<UpdateEnumValue>
    {
        private readonly IEntityRepository<EnumValue> _enumValueRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEnumValueHandler(IEntityRepository<EnumValue> enumValueRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _enumValueRepository = enumValueRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEnumValue request, CancellationToken cancellationToken)
        {
            var enumValue = await _enumValueRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (enumValue != null)
            {
                _mapper.Map(request, enumValue);
                await _unitOfWork.CommitAsync();
            }
            return Unit.Value;
        }

    }
}
