using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class UpdateEnumHandler : IRequestHandler<UpdateEnum>
    {
        private readonly CodeGenerator _codeGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public UpdateEnumHandler(CodeGenerator codeGenerator,
            IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _codeGenerator = codeGenerator;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEnum request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var newEnum = _mapper.Map<UpdateEnum, Generator.Models.Enum>(request);
            var oldEnum = (await solutionAccessor.GetEnumsAsync()).FirstOrDefault(e => e.Id == request.Id);
            await solutionAccessor.UpdateEnumAsync(newEnum);
            // reload Enum from Solution
            newEnum = (await solutionAccessor.GetEnumsAsync())
                .FirstOrDefault(e => e.Id == request.Id);
            await _codeGenerator.UpgradeEnumAsync(solutionAccessor, oldEnum, newEnum);
            await _codeGenerator.UpgradeEnumViewAsync(solutionAccessor, new Generator.Models.View(), oldEnum, newEnum);
            return Unit.Value;
        }

    }
}
