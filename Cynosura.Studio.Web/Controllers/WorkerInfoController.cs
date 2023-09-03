using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cynosura.Core.Services.Models;
using Cynosura.Web.Infrastructure;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.WorkerInfos;
using Cynosura.Studio.Core.Requests.WorkerInfos.Models;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [Authorize("ReadWorkerInfo")]
    [ValidateModel]
    [Route("api")]
    public class WorkerInfoController : Controller
    {
        private readonly IMediator _mediator;

        public WorkerInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetWorkerInfos")]
        public async Task<PageModel<WorkerInfoModel>> GetWorkerInfosAsync([FromBody] GetWorkerInfos getWorkerInfos)
        {
            return await _mediator.Send(getWorkerInfos);
        }

        [HttpPost("GetWorkerInfo")]
        public async Task<WorkerInfoModel> GetWorkerInfoAsync([FromBody] GetWorkerInfo getWorkerInfo)
        {
            return await _mediator.Send(getWorkerInfo);
        }

        [HttpPost("ExportWorkerInfos")]
        public async Task<FileResult> ExportWorkerInfosAsync([FromBody] ExportWorkerInfos exportWorkerInfos)
        {
            var file = await _mediator.Send(exportWorkerInfos);
            return File(file.Content, file.ContentType, file.Name);
        }

        [Authorize("WriteWorkerInfo")]
        [HttpPost("UpdateWorkerInfo")]
        public async Task<Unit> UpdateWorkerInfoAsync([FromBody] UpdateWorkerInfo updateWorkerInfo)
        {
            return await _mediator.Send(updateWorkerInfo);
        }

        [Authorize("WriteWorkerInfo")]
        [HttpPost("CreateWorkerInfo")]
        public async Task<CreatedEntity<int>> CreateWorkerInfoAsync([FromBody] CreateWorkerInfo createWorkerInfo)
        {
            return await _mediator.Send(createWorkerInfo);
        }

        [Authorize("WriteWorkerInfo")]
        [HttpPost("DeleteWorkerInfo")]
        public async Task<Unit> DeleteWorkerInfoAsync([FromBody] DeleteWorkerInfo deleteWorkerInfo)
        {
            return await _mediator.Send(deleteWorkerInfo);
        }
    }
}