using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.EnumValues.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class GetEnumValueHandler : IRequestHandler<GetEnumValue, EnumValueModel>
    {
        private readonly IEntityRepository<EnumValue> _enumValueRepository;
        private readonly IMapper _mapper;

        public GetEnumValueHandler(IEntityRepository<EnumValue> enumValueRepository,
            IMapper mapper)
        {
            _enumValueRepository = enumValueRepository;
            _mapper = mapper;
        }

        public async Task<EnumValueModel> Handle(GetEnumValue request, CancellationToken cancellationToken)
        {
            var enumValue = await _enumValueRepository.GetEntities()
                .Include(e => e.Enum)
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            return _mapper.Map<EnumValue, EnumValueModel>(enumValue);
        }

    }
}
