using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Web.Infrastructure.Menu;

namespace Cynosura.Studio.Web.Infrastructure.Menu
{
    public class EntityModule : IMenuModule
    {
        public IList<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>()
            {
                new MenuItem()
                {
                    Name = "Entities",
                    Route = "./entity",
                    CssClass = "glyphicon-folder-close",
                    Roles = new List<string>() {}
                }
            };
        }
    }
}