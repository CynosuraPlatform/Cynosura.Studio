using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.WorkerRuns.Models;

namespace Cynosura.Studio.Core.Requests.WorkerRuns
{
    public class GetWorkerRunHandler : IRequestHandler<GetWorkerRun, WorkerRunModel>
    {
        private readonly IEntityRepository<WorkerRun> _workerRunRepository;
        private readonly IMapper _mapper;

        public GetWorkerRunHandler(IEntityRepository<WorkerRun> workerRunRepository,
            IMapper mapper)
        {
            _workerRunRepository = workerRunRepository;
            _mapper = mapper;
        }

        public async Task<WorkerRunModel> Handle(GetWorkerRun request, CancellationToken cancellationToken)
        {
            var workerRun = await _workerRunRepository.GetEntities()
                .Include(e => e.WorkerInfo)
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            return _mapper.Map<WorkerRun, WorkerRunModel>(workerRun);
        }

    }
}
