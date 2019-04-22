using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Users.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class GetUsersHandler : IRequestHandler<GetUsers, PageModel<UserModel>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public GetUsersHandler(UserManager<User> userManager, IEntityRepository<User> userRepository, IMapper mapper)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PageModel<UserModel>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            IQueryable<User> query = _userManager.Users;
            query = query.Filter(request.Filter);
            query = query.OrderBy(request.OrderBy, request.OrderDirection);
            var users = await query.ToPagedListAsync(request.PageIndex, request.PageSize);
            return users.Map<User, UserModel>(_mapper);
        }

    }
}
