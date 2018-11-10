using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
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
    [Route("api/[controller]")]
    public class SolutionController : Controller
    {
        private readonly IMediator _mediator;

        public SolutionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<PageModel<SolutionModel>> GetSolutionsAsync(int? pageIndex, int? pageSize)
        {
            return await _mediator.Send(new GetSolutions() { PageIndex = pageIndex, PageSize = pageSize });
        }

        [HttpGet("{id:int}")]
        public async Task<SolutionModel> GetSolutionAsync(int id)
        {
            return await _mediator.Send(new GetSolution() { Id = id });
        }

        [HttpPut("{id:int}")]
        public async Task<StatusViewModel> PutSolutionAsync(int id, [FromBody] UpdateSolution updateSolution)
        {
            await _mediator.Send(updateSolution);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostSolutionAsync([FromBody] CreateSolution createSolution)
        {
            var id = await _mediator.Send(createSolution);
            return new CreationStatusViewModel(id);
        }

        [HttpDelete("{id:int}")]
        public async Task<StatusViewModel> DeleteSolutionAsync(int id)
        {
            await _mediator.Send(new DeleteSolution() { Id = id });
            return new StatusViewModel();
        }

        [HttpPost("{id:int}/generate")]
        public async Task<StatusViewModel> GenerateSolutionAsync(int id)
        {
            await _mediator.Send(new GenerateSolution() { Id = id });
            return new StatusViewModel();
        }

        [HttpPost("{id:int}/upgrade")]
        public async Task<StatusViewModel> UpgradeSolutionAsync(int id)
        {
            await _mediator.Send(new UpgradeSolution() { Id = id });
            return new StatusViewModel();
        }
    }
}