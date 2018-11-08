using System.Threading;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class DeleteUserHandler : IRequestHandler<DeleteUser>
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                result.CheckIfSucceeded();
            }
            return Unit.Value;
        }
    }
}
