using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Web.Infrastructure.Menu;

namespace Cynosura.Studio.Web.Infrastructure.Menu
{
    public class SolutionModule : IMenuModule
    {
        private readonly IEnumerable<MenuItem> _items = new[]
        {
            new MenuItem("./solution", "Solutions", "glyphicon-folder-close", new string[0])
        };

        public IEnumerable<MenuItem> GetMenuItems() => _items;
    }
}