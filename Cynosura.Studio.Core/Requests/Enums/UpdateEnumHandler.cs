using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class UpdateEnumHandler : IRequestHandler<UpdateEnum>
    {
        private readonly IEntityRepository<Enum> _enumRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEnumHandler(IEntityRepository<Enum> enumRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _enumRepository = enumRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEnum request, CancellationToken cancellationToken)
        {
            var @enum = await _enumRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (@enum != null)
            {
                _mapper.Map(request, @enum);
                await _unitOfWork.CommitAsync();
            }
            return Unit.Value;
        }

    }
}
