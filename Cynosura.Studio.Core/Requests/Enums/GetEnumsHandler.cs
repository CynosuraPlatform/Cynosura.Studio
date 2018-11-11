using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EnumModel = Cynosura.Studio.Core.Requests.Enums.Models.EnumModel;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnumsHandler : IRequestHandler<GetEnums, PageModel<EnumModel>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public GetEnumsHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<PageModel<EnumModel>> Handle(GetEnums request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var enums = await solutionAccessor.GetEnumsAsync();
            return enums.ToPagedList(request.PageIndex, request.PageSize)
                .Map<Generator.Models.Enum, EnumModel>(_mapper);
        }

    }
}
