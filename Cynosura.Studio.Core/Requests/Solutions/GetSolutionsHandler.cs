using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Solutions.Models;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GetSolutionsHandler : IRequestHandler<GetSolutions, PageModel<SolutionModel>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public GetSolutionsHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<PageModel<SolutionModel>> Handle(GetSolutions request, CancellationToken cancellationToken)
        {
            IQueryable<Solution> query = _solutionRepository.GetEntities();            
            query = query.Filter(request.Filter);
            query = query.OrderBy(request.OrderBy, request.OrderDirection);
            var solutions = await query.ToPagedListAsync(request.PageIndex, request.PageSize);
            var solutionsModels = solutions.Map<Solution, SolutionModel>(_mapper);
            var solutionPatch = new List<SolutionModel>();
            foreach (var solution in solutionsModels.PageItems)
            {
                try
                {
                    var accessor = new SolutionAccessor(solution.Path);
                    solution.TemplateName = accessor.Metadata.TemplateName;
                    solution.TemplateVersion = accessor.Metadata.TemplateVersion;
                }
                finally
                {
                    solutionPatch.Add(solution);
                }
            }

            solutionsModels.PageItems = solutionPatch;
            return solutionsModels;
        }

    }
}
