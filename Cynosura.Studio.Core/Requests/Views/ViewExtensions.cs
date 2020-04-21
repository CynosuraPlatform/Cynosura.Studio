using System;
using System.Linq;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Views.Models;

namespace Cynosura.Studio.Core.Requests.Views
{
    public static class ViewExtensions
    {
        public static IOrderedQueryable<View> OrderBy(this IQueryable<View> queryable, string propertyName, OrderDirection? direction)
        {
            switch (propertyName)
            {                
                case "Name":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Name)
                        : queryable.OrderBy(e => e.Name);
                case "":
                case null:
                    return queryable.OrderBy(e => e.Id);
                default:
                    throw new ArgumentException("Property not found", nameof(propertyName));
            }
        }

        public static IQueryable<View> Filter(this IQueryable<View> queryable, ViewFilter filter)
        {
            if (!string.IsNullOrEmpty(filter?.Text))
            {
                queryable = queryable.Where(e => e.Name.Contains(filter.Text));
            }
            if (!string.IsNullOrEmpty(filter?.Name))
            {
                queryable = queryable.Where(e => e.Name.Contains(filter.Name));
            }
            return queryable;
        }
    }
}
