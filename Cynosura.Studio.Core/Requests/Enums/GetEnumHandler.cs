using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EnumModel = Cynosura.Studio.Core.Requests.Enums.Models.EnumModel;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnumHandler : IRequestHandler<GetEnum, EnumModel>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public GetEnumHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<EnumModel> Handle(GetEnum request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var enums = await solutionAccessor.GetEnumsAsync();
            var @enum = enums.FirstOrDefault(e => e.Id == request.Id);
            return _mapper.Map<Generator.Models.Enum, EnumModel>(@enum);
        }

    }
}
