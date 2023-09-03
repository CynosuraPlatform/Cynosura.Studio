using System;
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
using Cynosura.Core.Services;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class CreateEnumHandler : IRequestHandler<CreateEnum, CreatedEntity<Guid>>
    {
        private readonly EnumGenerator _enumGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CreateEnumHandler(EnumGenerator enumGenerator,
            IEntityRepository<Solution> solutionRepository,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _enumGenerator = enumGenerator;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<CreatedEntity<Guid>> Handle(CreateEnum request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            if (solution == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Solution"], request.SolutionId]);
            }
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var @enum = _mapper.Map<CreateEnum, Generator.Models.Enum>(request);
            @enum.Id = Guid.NewGuid();
            await solutionAccessor.CreateEnumAsync(@enum);
            // reload Enum from Solution
            @enum = (await solutionAccessor.GetEnumsAsync())
                .First(e => e.Id == @enum.Id);
            await _enumGenerator.GenerateEnumAsync(solutionAccessor, @enum);
            await _enumGenerator.GenerateEnumViewAsync(solutionAccessor, @enum);
            return new CreatedEntity<Guid>(@enum.Id);
        }

    }
}
