using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator;
using Cynosura.Studio.Core.Generator.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class CreateEntityHandler : IRequestHandler<CreateEntity, Guid>
    {
        private readonly CodeGenerator _codeGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public CreateEntityHandler(CodeGenerator codeGenerator,
            IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _codeGenerator = codeGenerator;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateEntity request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var entity = _mapper.Map<CreateEntity, Generator.Models.Entity>(request);
            entity.Id = Guid.NewGuid();
            await solutionAccessor.CreateEntityAsync(entity);
            // reload Entity from Solution
            entity = (await solutionAccessor.GetEntitiesAsync())
                .First(e => e.Id == entity.Id);
            await _codeGenerator.GenerateAsync(solutionAccessor, entity, new EntityModel(entity, solutionAccessor), TemplateType.Entity);
            await _codeGenerator.GenerateAsync(solutionAccessor, entity,
                new ViewModel(new Generator.Models.View(), entity, solutionAccessor), TemplateType.View);
            return entity.Id;
        }

    }
}
