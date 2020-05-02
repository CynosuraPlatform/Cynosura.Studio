using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cynosura.Studio.Core.Requests.Account
{
    public class RegisterHandler : IRequestHandler<Register, CreatedEntity<int>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegisterHandler(UserManager<User> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CreatedEntity<int>> Handle(Register request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Register, User>(request);
            user.UserName = request.Email;
            var result = await _userManager.CreateAsync(user, request.Password);
            result.CheckIfSucceeded();

            return new CreatedEntity<int>() { Id = user.Id };

        }

    }
}
