using System;
using System.Linq;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Enums.Models;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public static class EnumExtensions
    {
        public static IOrderedQueryable<Core.Entities.Enum> OrderBy(this IQueryable<Core.Entities.Enum> queryable, string propertyName, OrderDirection? direction)
        {
            switch (propertyName)
            {                
                case "Name":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Name)
                        : queryable.OrderBy(e => e.Name);
                case "DisplayName":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.DisplayName)
                        : queryable.OrderBy(e => e.DisplayName);
                case "":
                case null:
                    return queryable.OrderBy(e => e.Id);
                default:
                    throw new ArgumentException("Property not found", nameof(propertyName));
            }
        }

        public static IQueryable<Core.Entities.Enum> Filter(this IQueryable<Core.Entities.Enum> queryable, EnumFilter filter)
        {
            if (!string.IsNullOrEmpty(filter?.Text))
            {
                queryable = queryable.Where(e => e.Name.Contains(filter.Text) || e.DisplayName.Contains(filter.Text));
            }
            if (!string.IsNullOrEmpty(filter?.Name))
            {
                queryable = queryable.Where(e => e.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrEmpty(filter?.DisplayName))
            {
                queryable = queryable.Where(e => e.DisplayName.Contains(filter.DisplayName));
            }
            return queryable;
        }
    }
}
