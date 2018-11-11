using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class CreateEnumHandler : IRequestHandler<CreateEnum, int>
    {
        private readonly IEntityRepository<Enum> _enumRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEnumHandler(IEntityRepository<Enum> enumRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _enumRepository = enumRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateEnum request, CancellationToken cancellationToken)
        {
            var @enum = _mapper.Map<CreateEnum, Enum>(request);
            _enumRepository.Add(@enum);
            await _unitOfWork.CommitAsync();
            return @enum.Id;
        }

    }
}
