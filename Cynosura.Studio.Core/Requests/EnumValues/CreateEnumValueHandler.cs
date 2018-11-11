using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using MediatR;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class CreateEnumValueHandler : IRequestHandler<CreateEnumValue, int>
    {
        private readonly IEntityRepository<EnumValue> _enumValueRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEnumValueHandler(IEntityRepository<EnumValue> enumValueRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _enumValueRepository = enumValueRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateEnumValue request, CancellationToken cancellationToken)
        {
            var enumValue = _mapper.Map<CreateEnumValue, EnumValue>(request);
            _enumValueRepository.Add(enumValue);
            await _unitOfWork.CommitAsync();
            return enumValue.Id;
        }

    }
}
