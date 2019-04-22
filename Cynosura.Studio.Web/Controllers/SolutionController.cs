using System;
using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Solutions;
using Cynosura.Studio.Core.Requests.Solutions.Models;
using Cynosura.Studio.Web.Models;
using Cynosura.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [ValidateModel]
    [Route("api")]
    public class SolutionController : Controller
    {
        private readonly IMediator _mediator;

        public SolutionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetSolutions")]
        public async Task<PageModel<SolutionModel>> GetSolutionsAsync([FromBody] GetSolutions getSolutions)
        {
            return await _mediator.Send(getSolutions);
        }

        [HttpPost("GetSolution")]
        public async Task<SolutionModel> GetSolutionAsync([FromBody] GetSolution getSolution)
        {
            return await _mediator.Send(getSolution);
        }

        [HttpPost("UpdateSolution")]
        public async Task<Unit> UpdateSolutionAsync([FromBody] UpdateSolution updateSolution)
        {
            return await _mediator.Send(updateSolution);
        }

        [HttpPost("CreateSolution")]
        public async Task<CreatedEntity<int>> CreateSolutionAsync([FromBody] CreateSolution createSolution)
        {
            return await _mediator.Send(createSolution);
        }

        [HttpPost("DeleteSolution")]
        public async Task<Unit> DeleteSolutionAsync([FromBody] DeleteSolution deleteSolution)
        {
            return await _mediator.Send(deleteSolution);
        }

        [HttpPost("GenerateSolution")]
        public async Task<Unit> GenerateSolutionAsync([FromBody] GenerateSolution generateSolution)
        {
            return await _mediator.Send(generateSolution);
        }

        [HttpPost("UpgradeSolution")]
        public async Task<Unit> UpgradeSolutionAsync([FromBody] UpgradeSolution upgradeSolution)
        {
            return await _mediator.Send(upgradeSolution);
        }
    }
}
