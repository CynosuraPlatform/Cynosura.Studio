using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Users;
using Cynosura.Studio.Core.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Profile
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfile>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoProvider _userInfoProvider;

        public UpdateProfileHandler(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IUserInfoProvider userInfoProvider)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userInfoProvider = userInfoProvider;
        }

        public async Task<Unit> Handle(UpdateProfile request, CancellationToken cancellationToken)
        {
            var userInfo = await _userInfoProvider.GetUserInfoAsync();
            var user = await _userManager.Users
                .FirstOrDefaultAsync(e => e.Id == userInfo.UserId, cancellationToken);

            if (!string.Equals(user.Email, request.Email, StringComparison.CurrentCultureIgnoreCase))
            {
                await _userManager.UpdateUserEmail(user, request.Email);
            }

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword,
                    request.NewPassword);
                result.CheckIfSucceeded();
            }

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
