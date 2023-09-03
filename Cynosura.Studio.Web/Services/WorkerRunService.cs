﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.WorkerRuns;
using Cynosura.Studio.Core.Requests.WorkerRuns.Models;
using Cynosura.Studio.Web.Protos;
using Cynosura.Studio.Web.Protos.WorkerRuns;

namespace Cynosura.Studio.Web.Services
{
    [Authorize("ReadWorkerRun")]
    public class WorkerRunService : Protos.WorkerRuns.WorkerRunService.WorkerRunServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public WorkerRunService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<WorkerRunPageModel> GetWorkerRuns(GetWorkerRunsRequest getWorkerRunsRequest, ServerCallContext context)
        {
            var getWorkerRuns = _mapper.Map<GetWorkerRunsRequest, GetWorkerRuns>(getWorkerRunsRequest);
            return _mapper.Map<PageModel<WorkerRunModel>, WorkerRunPageModel>(await _mediator.Send(getWorkerRuns));
        }

        public override async Task<WorkerRun> GetWorkerRun(GetWorkerRunRequest getWorkerRunRequest, ServerCallContext context)
        {
            var getWorkerRun = _mapper.Map<GetWorkerRunRequest, GetWorkerRun>(getWorkerRunRequest);
            return _mapper.Map<WorkerRunModel, WorkerRun>(await _mediator.Send(getWorkerRun));
        }

        [Authorize("WriteWorkerRun")]
        public override async Task<CreatedEntity> CreateWorkerRun(CreateWorkerRunRequest createWorkerRunRequest, ServerCallContext context)
        {
            var createWorkerRun = _mapper.Map<CreateWorkerRunRequest, CreateWorkerRun>(createWorkerRunRequest);
            return _mapper.Map<CreatedEntity<int>, CreatedEntity>(await _mediator.Send(createWorkerRun));
        }

        [Authorize("WriteWorkerRun")]
        public override async Task<Empty> DeleteWorkerRun(DeleteWorkerRunRequest deleteWorkerRunRequest, ServerCallContext context)
        {
            var deleteWorkerRun = _mapper.Map<DeleteWorkerRunRequest, DeleteWorkerRun>(deleteWorkerRunRequest);
            return _mapper.Map<Unit, Empty>(await _mediator.Send(deleteWorkerRun));
        }
    }
}