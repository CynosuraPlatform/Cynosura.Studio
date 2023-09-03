using System;
using System.Linq;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Entities.Models;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public static class EntityExtensions
    {
        public static IOrderedQueryable<Entity> OrderBy(this IQueryable<Entity> queryable, string? propertyName, OrderDirection? direction)
        {
            switch (propertyName)
            {                
                case "Name":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Name)
                        : queryable.OrderBy(e => e.Name);
                case "PluralName":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.PluralName)
                        : queryable.OrderBy(e => e.PluralName);
                case "DisplayName":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.DisplayName)
                        : queryable.OrderBy(e => e.DisplayName);
                case "PluralDisplayName":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.PluralDisplayName)
                        : queryable.OrderBy(e => e.PluralDisplayName);
                case "IsAbstract":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.IsAbstract)
                        : queryable.OrderBy(e => e.IsAbstract);
                case "BaseEntity":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.BaseEntity)
                        : queryable.OrderBy(e => e.BaseEntity);
                case "":
                case null:
                    return queryable.OrderBy(e => e.Id);
                default:
                    throw new ArgumentException("Property not found", nameof(propertyName));
            }
        }

        public static IQueryable<Entity> Filter(this IQueryable<Entity> queryable, Models.EntityFilter filter)
        {
            if (!string.IsNullOrEmpty(filter?.Text))
            {
                queryable = queryable.Where(e => e.Name.Contains(filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                               e.PluralName.Contains(filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                               e.DisplayName.Contains(filter.Text, StringComparison.CurrentCultureIgnoreCase) ||
                                               e.PluralDisplayName.Contains(filter.Text, StringComparison.CurrentCultureIgnoreCase));
            }
            if (!string.IsNullOrEmpty(filter?.Name))
            {
                queryable = queryable.Where(e => e.Name!.Contains(filter.Name));
            }
            if (!string.IsNullOrEmpty(filter?.PluralName))
            {
                queryable = queryable.Where(e => e.PluralName!.Contains(filter.PluralName));
            }
            if (!string.IsNullOrEmpty(filter?.DisplayName))
            {
                queryable = queryable.Where(e => e.DisplayName!.Contains(filter.DisplayName));
            }
            if (!string.IsNullOrEmpty(filter?.PluralDisplayName))
            {
                queryable = queryable.Where(e => e.PluralDisplayName!.Contains(filter.PluralDisplayName));
            }
            if (filter?.IsAbstract != null)
            {
                queryable = queryable.Where(e => e.IsAbstract == filter.IsAbstract);
            }
            if (filter?.BaseEntityId != null)
            {
                queryable = queryable.Where(e => e.BaseEntityId == filter.BaseEntityId);
            }
            return queryable;
        }
    }
}
