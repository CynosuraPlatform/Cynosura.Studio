﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using EnumModel = Cynosura.Studio.Core.Requests.Enums.Models.EnumModel;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class GetEnumsHandler : IRequestHandler<GetEnums, PageModel<EnumModel>>
    {
        private readonly IEntityRepository<Solution> _solutionRepository;
        private readonly IMapper _mapper;

        public GetEnumsHandler(IEntityRepository<Solution> solutionRepository,
            IMapper mapper)
        {
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }

        public async Task<PageModel<EnumModel>> Handle(GetEnums request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .Where(e => e.Id == request.SolutionId)
                .FirstOrDefaultAsync();
            var solutionAccessor = new SolutionAccessor(solution.Path);
            var enums = await solutionAccessor.GetEnumsAsync();
            if (!string.IsNullOrEmpty(request.Filter?.Text))
            {
                enums = enums.Where(e => e.Name.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase) || 
                                         e.DisplayName.Contains(request.Filter.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            return enums.ToPagedList(request.PageIndex, request.PageSize)
                .Map<Generator.Models.Enum, EnumModel>(_mapper);
        }

    }
}
