using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cynosura.Core.Services.Models;

namespace Cynosura.Studio.Core
{
    public static class PageModelExtensions
    {
        public static PageModel<T> ToPagedList<T>(this IEnumerable<T> enumerable, int? pageIndex = null, int? pageSize = null)
        {
            var list = enumerable.ToList();
            if (pageIndex != null && pageSize != null)
            {
                var pageList = list.Skip(pageIndex.Value * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();
                return new PageModel<T>(pageList, list.Count, pageIndex.Value);
            }
            else
            {
                return new PageModel<T>(list, list.Count, 0);
            }
        }
    }
}
