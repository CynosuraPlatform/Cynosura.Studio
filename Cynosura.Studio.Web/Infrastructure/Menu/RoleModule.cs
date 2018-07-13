using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Web.Infrastructure.Menu;

namespace Cynosura.Studio.Web.Infrastructure.Menu
{
    public class RoleModule : IMenuModule
    {
        public IList<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>()
            {
                new MenuItem()
                {
                    Name = "Roles",
                    Route = "./role",
                    CssClass = "glyphicon-lock",
                    Roles = new List<string>() { "Administrator" }
                }
            };
        }
    }
}
