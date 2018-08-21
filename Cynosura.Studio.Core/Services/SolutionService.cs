using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Microsoft.EntityFrameworkCore;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator;
using Cynosura.Studio.Core.Services.Models;

namespace Cynosura.Studio.Core.Services
{
    public class SolutionService : ISolutionService
    {
        private readonly CodeGenerator _codeGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SolutionService(CodeGenerator codeGenerator,
            IEntityRepository<Solution> solutionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _codeGenerator = codeGenerator;
            _solutionRepository = solutionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Solution> GetSolutionAsync(int id)
        {
            return await _solutionRepository.GetEntities()
                .Where(e => e.Id == id)
				.FirstOrDefaultAsync();
        }

        public async Task<PageModel<Solution> > GetSolutionsAsync(int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Solution> query = _solutionRepository.GetEntities();
            query = query.OrderBy(e => e.Id);
            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        public async Task<int> CreateSolutionAsync(SolutionCreateModel model)
        {
            var solution = _mapper.Map<SolutionCreateModel, Solution>(model);
            _solutionRepository.Add(solution);
            await _unitOfWork.CommitAsync();
            return solution.Id;
        }

        public async Task UpdateSolutionAsync(int id, SolutionUpdateModel model)
        {
            var solution = await GetSolutionAsync(id);
            if (solution == null)
                return;
            _mapper.Map(model, solution);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteSolutionAsync(int id)
        {
            var solution = await GetSolutionAsync(id);
            if (solution == null)
                return;
            _solutionRepository.Delete(solution);
            await _unitOfWork.CommitAsync();
        }

        public async Task GenerateAsync(int id)
        {
            var solution = await GetSolutionAsync(id);
            if (solution == null)
                return;
            _codeGenerator.GenerateSolution(solution.Path, solution.Name);
        }

        public async Task UpgradeAsync(int id)
        {
            var solution = await GetSolutionAsync(id);
            if (solution == null)
                return;
            await _codeGenerator.UpgradeSolutionAsync(new Generator.Models.Solution(solution.Path));
        }
    }
}
