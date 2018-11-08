using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class UpdateSolutionHandler : IRequestHandler<UpdateSolution>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSolutionHandler(IEntityRepository<Solution> solutionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateSolution request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (solution != null)
            {
                _mapper.Map(request, solution);
                await _unitOfWork.CommitAsync();
            }
            return Unit.Value;
        }

    }
}
