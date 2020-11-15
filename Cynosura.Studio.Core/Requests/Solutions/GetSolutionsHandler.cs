using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Solutions.Models;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GetSolutionsHandler : IRequestHandler<GetSolutions, PageModel<SolutionModel>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly ILogger<GetSolutionsHandler> _logger;
        private readonly IMapper _mapper;

        public GetSolutionsHandler(IEntityRepository<Solution> solutionRepository,
            ILogger<GetSolutionsHandler> logger,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PageModel<SolutionModel>> Handle(GetSolutions request, CancellationToken cancellationToken)
        {
            IQueryable<Solution> query = _solutionRepository.GetEntities();            
            query = query.Filter(request.Filter);
            query = query.OrderBy(request.OrderBy, request.OrderDirection);
            var solutions = await query.ToPagedListAsync(request.PageIndex, request.PageSize);
            var solutionsModels = solutions.Map<Solution, SolutionModel>(_mapper);
            solutionsModels.PageItems = solutionsModels.PageItems
                .Select(s =>
                {
                    try
                    {
                        s.LoadMetadata();
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(e, "Can't load metadata for {0}", s.Name);
                    }
                    return s;
                })
                .ToList();
            return solutionsModels;
        }

    }
}
