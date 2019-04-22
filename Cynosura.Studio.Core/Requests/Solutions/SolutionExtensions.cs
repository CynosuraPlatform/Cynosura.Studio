using System;
using System.Linq;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Solutions.Models;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public static class SolutionExtensions
    {
        public static IOrderedQueryable<Solution> OrderBy(this IQueryable<Solution> queryable, string propertyName, OrderDirection? direction)
        {
            switch (propertyName)
            {                
                case "Name":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Name)
                        : queryable.OrderBy(e => e.Name);
                case "Path":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Path)
                        : queryable.OrderBy(e => e.Path);
                case "":
                case null:
                    return queryable.OrderBy(e => e.Id);
                default:
                    throw new ArgumentException("Property not found", nameof(propertyName));
            }
        }

        public static IQueryable<Solution> Filter(this IQueryable<Solution> queryable, SolutionFilter filter)
        {
            if (!string.IsNullOrEmpty(filter?.Text))
            {
                queryable = queryable.Where(e => e.Name.Contains(filter.Text) || e.Path.Contains(filter.Text));
            }
            if (!string.IsNullOrEmpty(filter?.Name))
            {
                queryable = queryable.Where(e => e.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrEmpty(filter?.Path))
            {
                queryable = queryable.Where(e => e.Path.Contains(filter.Path));
            }
            return queryable;
        }
    }
}
