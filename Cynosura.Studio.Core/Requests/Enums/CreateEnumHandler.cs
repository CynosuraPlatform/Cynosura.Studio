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

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class CreateEnumHandler : IRequestHandler<CreateEnum, CreatedEntity<Guid>>
    {
        private readonly EnumGenerator _enumGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public CreateEnumHandler(EnumGenerator enumGenerator,
            IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _enumGenerator = enumGenerator;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<CreatedEntity<Guid>> Handle(CreateEnum request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var @enum = _mapper.Map<CreateEnum, Generator.Models.Enum>(request);
            @enum.Id = Guid.NewGuid();
            await solutionAccessor.CreateEnumAsync(@enum);
            // reload Enum from Solution
            @enum = (await solutionAccessor.GetEnumsAsync())
                .First(e => e.Id == @enum.Id);
            await _enumGenerator.GenerateEnumAsync(solutionAccessor, @enum);
            await _enumGenerator.GenerateEnumViewAsync(solutionAccessor, @enum);
            return new CreatedEntity<Guid>() { Id = @enum.Id };
        }

    }
}
