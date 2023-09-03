using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Formatters;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Entities.Models;
using Cynosura.Studio.Generator;
using EntityModel = Cynosura.Studio.Core.Requests.Entities.Models.EntityModel;
using Cynosura.Core.Services;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class ExportEntitiesHandler : IRequestHandler<ExportEntities, FileContentModel>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IExcelFormatter _excelFormatter;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ExportEntitiesHandler(IEntityRepository<Solution> solutionRepository,
            IExcelFormatter excelFormatter,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _solutionRepository = solutionRepository;
            _excelFormatter = excelFormatter;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<FileContentModel> Handle(ExportEntities request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            if (solution == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["Solution"], request.SolutionId]);
            }
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var entities = await solutionAccessor.GetEntitiesAsync();
            if (!string.IsNullOrEmpty(request.Filter?.Text))
            {
                entities = entities.Where(e => e.Name.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                               e.PluralName.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                               e.DisplayName.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                               e.PluralDisplayName.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            var models = _mapper.Map<List<Generator.Models.Entity>, List<EntityModel>>(entities);
            return await _excelFormatter.GetExcelFileAsync(models, "Entities");
        }

    }
}
