using System;
using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Views;
using Cynosura.Studio.Core.Requests.Views.Models;
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
    public class ViewController : Controller
    {
        private readonly IMediator _mediator;

        public ViewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetViews")]
        public async Task<PageModel<ViewModel>> GetViewsAsync([FromBody] GetViews getViews)
        {
            return await _mediator.Send(getViews);
        }

        [HttpPost("GetView")]
        public async Task<ViewModel> GetViewAsync([FromBody] GetView getView)
        {
            return await _mediator.Send(getView);
        }

        [HttpPost("UpdateView")]
        public async Task<Unit> UpdateViewAsync([FromBody] UpdateView updateView)
        {
            return await _mediator.Send(updateView);
        }

        [HttpPost("CreateView")]
        public async Task<CreatedEntity<Guid>> CreateViewAsync([FromBody] CreateView createView)
        {
            return await _mediator.Send(createView);
        }

        [HttpPost("DeleteView")]
        public async Task<Unit> DeleteViewAsync([FromBody] DeleteView deleteView)
        {
            return await _mediator.Send(deleteView);
        }
    }
}
