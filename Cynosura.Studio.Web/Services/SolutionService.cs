using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Solutions;
using Cynosura.Studio.Core.Requests.Solutions.Models;
using Cynosura.Studio.Web.Protos;
using Cynosura.Studio.Web.Protos.Solutions;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cynosura.Studio.Web.Services
{
    [Authorize("ReadSolution")]
    public class SolutionService : Protos.Solutions.SolutionService.SolutionServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SolutionService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<SolutionPageModel> GetSolutions(GetSolutionsRequest getSolutionsRequest, ServerCallContext context)
        {
            var getSolutions = _mapper.Map<GetSolutionsRequest, GetSolutions>(getSolutionsRequest);
            return _mapper.Map<PageModel<SolutionModel>, SolutionPageModel>(await _mediator.Send(getSolutions));
        }

        public override async Task<Solution> GetSolution(GetSolutionRequest getSolutionRequest, ServerCallContext context)
        {
            var getSolution = _mapper.Map<GetSolutionRequest, GetSolution>(getSolutionRequest);
            return _mapper.Map<SolutionModel, Solution>(await _mediator.Send(getSolution));
        }

        [Authorize("WriteSolution")]
        public override async Task<Empty> UpdateSolution(UpdateSolutionRequest updateSolutionRequest, ServerCallContext context)
        {
            var updateSolution = _mapper.Map<UpdateSolutionRequest, UpdateSolution>(updateSolutionRequest);
            return _mapper.Map<Unit, Empty>(await _mediator.Send(updateSolution));
        }

        [Authorize("WriteSolution")]
        public override async Task<CreatedEntity> CreateSolution(CreateSolutionRequest createSolutionRequest, ServerCallContext context)
        {
            var createSolution = _mapper.Map<CreateSolutionRequest, CreateSolution>(createSolutionRequest);
            return _mapper.Map<CreatedEntity<int>, CreatedEntity>(await _mediator.Send(createSolution));
        }

        [Authorize("WriteSolution")]
        public override async Task<Empty> DeleteSolution(DeleteSolutionRequest deleteSolutionRequest, ServerCallContext context)
        {
            var deleteSolution = _mapper.Map<DeleteSolutionRequest, DeleteSolution>(deleteSolutionRequest);
            return _mapper.Map<Unit, Empty>(await _mediator.Send(deleteSolution));
        }
    }
}
