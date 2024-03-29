﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Formatters;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Users.Models;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class ExportUsersHandler : IRequestHandler<ExportUsers, FileContentModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IExcelFormatter _excelFormatter;
        private readonly IMapper _mapper;

        public ExportUsersHandler(UserManager<User> userManager, IExcelFormatter excelFormatter, IMapper mapper)
        {
            _userManager = userManager;
            _excelFormatter = excelFormatter;
            _mapper = mapper;
        }

        public async Task<FileContentModel> Handle(ExportUsers request, CancellationToken cancellationToken)
        {
            IQueryable<User> query = _userManager.Users;
            query = query.Filter(request.Filter);
            query = query.OrderBy(request.OrderBy, request.OrderDirection);
            var users = await query.ToListAsync(cancellationToken);
            var models = _mapper.Map<List<User>, List<UserModel>>(users);
            return await _excelFormatter.GetExcelFileAsync(models, "Users");
        }

    }
}
