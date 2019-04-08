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

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class CreateEnumHandler : IRequestHandler<CreateEnum, Guid>
    {
        private readonly CodeGenerator _codeGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public CreateEnumHandler(CodeGenerator codeGenerator,
            IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _codeGenerator = codeGenerator;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateEnum request, CancellationToken cancellationToken)
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
            await _codeGenerator.GenerateAsync(solutionAccessor, @enum, new EnumModel(@enum, solutionAccessor), TemplateType.Enum);
            await _codeGenerator.GenerateAsync(solutionAccessor, @enum,
                new EnumViewModel(new Generator.Models.View(), @enum, solutionAccessor), TemplateType.EnumView);
            return @enum.Id;
        }

    }
}
