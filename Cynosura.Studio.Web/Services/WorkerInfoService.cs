using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.WorkerInfos;
using Cynosura.Studio.Core.Requests.WorkerInfos.Models;
using Cynosura.Studio.Web.Protos;
using Cynosura.Studio.Web.Protos.WorkerInfos;

namespace Cynosura.Studio.Web.Services
{
    [Authorize("ReadWorkerInfo")]
    public class WorkerInfoService : Protos.WorkerInfos.WorkerInfoService.WorkerInfoServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public WorkerInfoService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<WorkerInfoPageModel> GetWorkerInfos(GetWorkerInfosRequest getWorkerInfosRequest, ServerCallContext context)
        {
            var getWorkerInfos = _mapper.Map<GetWorkerInfosRequest, GetWorkerInfos>(getWorkerInfosRequest);
            return _mapper.Map<PageModel<WorkerInfoModel>, WorkerInfoPageModel>(await _mediator.Send(getWorkerInfos));
        }

        public override async Task<WorkerInfo> GetWorkerInfo(GetWorkerInfoRequest getWorkerInfoRequest, ServerCallContext context)
        {
            var getWorkerInfo = _mapper.Map<GetWorkerInfoRequest, GetWorkerInfo>(getWorkerInfoRequest);
            return _mapper.Map<WorkerInfoModel, WorkerInfo>(await _mediator.Send(getWorkerInfo));
        }

        [Authorize("WriteWorkerInfo")]
        public override async Task<Empty> UpdateWorkerInfo(UpdateWorkerInfoRequest updateWorkerInfoRequest, ServerCallContext context)
        {
            var updateWorkerInfo = _mapper.Map<UpdateWorkerInfoRequest, UpdateWorkerInfo>(updateWorkerInfoRequest);
            return _mapper.Map<Unit, Empty>(await _mediator.Send(updateWorkerInfo));
        }

        [Authorize("WriteWorkerInfo")]
        public override async Task<CreatedEntity> CreateWorkerInfo(CreateWorkerInfoRequest createWorkerInfoRequest, ServerCallContext context)
        {
            var createWorkerInfo = _mapper.Map<CreateWorkerInfoRequest, CreateWorkerInfo>(createWorkerInfoRequest);
            return _mapper.Map<CreatedEntity<int>, CreatedEntity>(await _mediator.Send(createWorkerInfo));
        }

        [Authorize("WriteWorkerInfo")]
        public override async Task<Empty> DeleteWorkerInfo(DeleteWorkerInfoRequest deleteWorkerInfoRequest, ServerCallContext context)
        {
            var deleteWorkerInfo = _mapper.Map<DeleteWorkerInfoRequest, DeleteWorkerInfo>(deleteWorkerInfoRequest);
            return _mapper.Map<Unit, Empty>(await _mediator.Send(deleteWorkerInfo));
        }
    }
}
