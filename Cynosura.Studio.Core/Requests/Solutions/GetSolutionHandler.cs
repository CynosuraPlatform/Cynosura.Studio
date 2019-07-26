using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Solutions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GetSolutionHandler : IRequestHandler<GetSolution, SolutionModel>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly ILogger<GetSolutionHandler> _logger;
        private readonly IMapper _mapper;

        public GetSolutionHandler(IEntityRepository<Solution> solutionRepository,
            ILogger<GetSolutionHandler> logger,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SolutionModel> Handle(GetSolution request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            var solutionModel = _mapper.Map<Solution, SolutionModel>(solution);
            if (solutionModel != null)
            {
                try
                {
                    solutionModel.LoadMetadata();
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Can't load metadata for {0}", solution.Name);
                }
            }
            return solutionModel;
        }

    }
}
