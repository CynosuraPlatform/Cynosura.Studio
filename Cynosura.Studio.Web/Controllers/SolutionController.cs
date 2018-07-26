using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Web.Infrastructure;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models;
using Cynosura.Studio.Web.Models.SolutionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [ValidateModel]
    [Route("api/[controller]")]
    public class SolutionController : Controller
    {
        private readonly ISolutionService _solutionService;
        private readonly IMapper _mapper;

        public SolutionController(ISolutionService solutionService, IMapper mapper)
        {
            _solutionService = solutionService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<PageModel<SolutionViewModel> > GetSolutionsAsync(int? pageIndex, int? pageSize)
        {
            var solutions = await _solutionService.GetSolutionsAsync(pageIndex, pageSize);
            return solutions.Map<Solution, SolutionViewModel>(_mapper);
        }

        [HttpGet("{id:int}")]
        public async Task<SolutionViewModel> GetSolutionAsync(int id)
        {
            var solution = await _solutionService.GetSolutionAsync(id);
            return _mapper.Map<Solution, SolutionViewModel>(solution);
        }

        [HttpPut("{id:int}")]
        public async Task<StatusViewModel> PutSolutionAsync(int id, [FromBody] SolutionUpdateViewModel solution)
        {
            var model = _mapper.Map<SolutionUpdateViewModel, SolutionUpdateModel>(solution);
            await _solutionService.UpdateSolutionAsync(id, model);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostSolutionAsync([FromBody] SolutionCreateViewModel solution)
        {
            var model = _mapper.Map<SolutionCreateViewModel, SolutionCreateModel>(solution);
            await _solutionService.CreateSolutionAsync(model);
            return new StatusViewModel();
        }

        [HttpDelete("{id:int}")]
        public async Task<StatusViewModel> DeleteSolutionAsync(int id)
        {
            await _solutionService.DeleteSolutionAsync(id);
            return new StatusViewModel();
        }
    }
}