using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.Security
{
    public interface IUserInfoProvider
    {
        Task<UserInfoModel> GetUserInfoAsync();
    }
}
