using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.EnumValues.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class GetEnumValuesHandler : IRequestHandler<GetEnumValues, PageModel<EnumValueModel>>
    {
        private readonly IEntityRepository<EnumValue> _enumValueRepository;
        private readonly IMapper _mapper;

        public GetEnumValuesHandler(IEntityRepository<EnumValue> enumValueRepository,
            IMapper mapper)
        {
            _enumValueRepository = enumValueRepository;
            _mapper = mapper;
        }

        public async Task<PageModel<EnumValueModel>> Handle(GetEnumValues request, CancellationToken cancellationToken)
        {
            IQueryable<EnumValue> query = _enumValueRepository.GetEntities()
                .Include(e => e.Enum);
            query = query.OrderBy(e => e.Id);
            var enumValues = await query.ToPagedListAsync(request.PageIndex, request.PageSize);
            return enumValues.Map<EnumValue, EnumValueModel>(_mapper);
        }

    }
}
