﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.WorkerInfos.Models;

namespace Cynosura.Studio.Core.Requests.WorkerInfos
{
    public class GetWorkerInfoHandler : IRequestHandler<GetWorkerInfo, WorkerInfoModel?>
    {
        private readonly IEntityRepository<WorkerInfo> _workerInfoRepository;
        private readonly IMapper _mapper;

        public GetWorkerInfoHandler(IEntityRepository<WorkerInfo> workerInfoRepository,
            IMapper mapper)
        {
            _workerInfoRepository = workerInfoRepository;
            _mapper = mapper;
        }

        public async Task<WorkerInfoModel?> Handle(GetWorkerInfo request, CancellationToken cancellationToken)
        {
            var workerInfo = await _workerInfoRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (workerInfo == null)
            {
                return null;
            }
            return _mapper.Map<WorkerInfo, WorkerInfoModel>(workerInfo);
        }

    }
}
