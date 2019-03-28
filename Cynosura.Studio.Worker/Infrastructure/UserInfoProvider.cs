using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Security;

namespace Cynosura.Studio.Worker.Infrastructure
{
    public class UserInfoProvider : IUserInfoProvider
    {
        public Task<UserInfoModel> GetUserInfoAsync()
        {
            return Task.FromResult(new UserInfoModel());
        }
    }
}
