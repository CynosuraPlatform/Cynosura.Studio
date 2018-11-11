using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Enums.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnumsHandler : IRequestHandler<GetEnums, PageModel<EnumModel>>
    {
        private readonly IEntityRepository<Enum> _enumRepository;
        private readonly IMapper _mapper;

        public GetEnumsHandler(IEntityRepository<Enum> enumRepository,
            IMapper mapper)
        {
            _enumRepository = enumRepository;
            _mapper = mapper;
        }

        public async Task<PageModel<EnumModel>> Handle(GetEnums request, CancellationToken cancellationToken)
        {
            IQueryable<Enum> query = _enumRepository.GetEntities();
            query = query.OrderBy(e => e.Id);
            var enums = await query.ToPagedListAsync(request.PageIndex, request.PageSize);
            return enums.Map<Enum, EnumModel>(_mapper);
        }

    }
}
