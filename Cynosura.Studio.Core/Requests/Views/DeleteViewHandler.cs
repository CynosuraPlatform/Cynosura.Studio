using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Cynosura.Core.Data;
using Cynosura.Core.Services;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class DeleteViewHandler : IRequestHandler<DeleteView>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly ViewGenerator _viewGenerator;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DeleteViewHandler(IEntityRepository<Solution> solutionRepository,
            ViewGenerator viewGenerator,
            IStringLocalizer<SharedResource> localizer)
        {
            _solutionRepository = solutionRepository;
            _viewGenerator = viewGenerator;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteView request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var view = (await solutionAccessor.GetViewsAsync()).FirstOrDefault(e => e.Id == request.Id);
            if (view == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["View"], request.Id]);
            }
            await _viewGenerator.DeleteViewAsync(solutionAccessor, view);
            await solutionAccessor.DeleteViewAsync(request.Id);
            return Unit.Value;
        }

    }
}
