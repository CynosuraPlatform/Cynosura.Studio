﻿using System;
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
using Cynosura.Studio.Core.Requests.Enums.Models;
using Cynosura.Studio.Generator;
using EnumModel = Cynosura.Studio.Core.Requests.Enums.Models.EnumModel;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class ExportEnumsHandler : IRequestHandler<ExportEnums, FileContentModel>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IExcelFormatter _excelFormatter;
        private readonly IMapper _mapper;

        public ExportEnumsHandler(IEntityRepository<Solution> solutionRepository,
            IExcelFormatter excelFormatter,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _excelFormatter = excelFormatter;
            _mapper = mapper;
        }

        public async Task<FileContentModel> Handle(ExportEnums request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var enums = await solutionAccessor.GetEnumsAsync();
            if (!string.IsNullOrEmpty(request.Filter?.Text))
            {
                enums = enums.Where(e => e.Name.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                         e.DisplayName.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            var models = _mapper.Map<List<Generator.Models.Enum>, List<EnumModel>>(enums);
            return await _excelFormatter.GetExcelFileAsync(models, "Enums");
        }

    }
}
