using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Enums.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnumHandler : IRequestHandler<GetEnum, EnumModel>
    {
        private readonly IEntityRepository<Enum> _enumRepository;
        private readonly IMapper _mapper;

        public GetEnumHandler(IEntityRepository<Enum> enumRepository,
            IMapper mapper)
        {
            _enumRepository = enumRepository;
            _mapper = mapper;
        }

        public async Task<EnumModel> Handle(GetEnum request, CancellationToken cancellationToken)
        {
            var @enum = await _enumRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            return _mapper.Map<Enum, EnumModel>(@enum);
        }

    }
}
