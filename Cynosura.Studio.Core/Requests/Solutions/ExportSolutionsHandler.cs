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
using Cynosura.Studio.Core.Requests.Solutions.Models;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class ExportSolutionsHandler : IRequestHandler<ExportSolutions, FileContentModel>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IExcelFormatter _excelFormatter;
        private readonly IMapper _mapper;

        public ExportSolutionsHandler(IEntityRepository<Solution> solutionRepository,
            IExcelFormatter excelFormatter,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _excelFormatter = excelFormatter;
            _mapper = mapper;
        }

        public async Task<FileContentModel> Handle(ExportSolutions request, CancellationToken cancellationToken)
        {
            IQueryable<Solution> query = _solutionRepository.GetEntities();            
            query = query.Filter(request.Filter);
            query = query.OrderBy(request.OrderBy, request.OrderDirection);
            var solutions = await query.ToListAsync(cancellationToken);
            var models = _mapper.Map<List<Solution>, List<SolutionModel>>(solutions);
            return await _excelFormatter.GetExcelFileAsync(models, "Solutions");
        }

    }
}
