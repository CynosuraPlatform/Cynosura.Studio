﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Generator;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class CreateEntityHandler : IRequestHandler<CreateEntity, CreatedEntity<Guid>>
    {
        private readonly EntityGenerator _entityGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public CreateEntityHandler(EntityGenerator EntityGenerator,
            IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _entityGenerator = EntityGenerator;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<CreatedEntity<Guid>> Handle(CreateEntity request, CancellationToken cancellationToken)
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
            await _entityGenerator.GenerateEntityAsync(solutionAccessor, entity);
            await _entityGenerator.GenerateEntityViewAsync(solutionAccessor, entity);
            return new CreatedEntity<Guid>() { Id = entity.Id };
        }

    }
}
