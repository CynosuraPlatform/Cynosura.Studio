using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Core.Entities;

namespace Cynosura.Studio.Core.Security
{
    public class UserInfoModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
        public int? UserId => User?.Id;
    }
}
