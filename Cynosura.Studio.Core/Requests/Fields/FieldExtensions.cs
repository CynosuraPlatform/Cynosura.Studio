using System;
using System.Linq;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Fields.Models;

namespace Cynosura.Studio.Core.Requests.Fields
{
    public static class FieldExtensions
    {
        public static IOrderedQueryable<Field> OrderBy(this IQueryable<Field> queryable, string propertyName, OrderDirection? direction)
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
                case "Size":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Size)
                        : queryable.OrderBy(e => e.Size);
                case "Entity":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Entity)
                        : queryable.OrderBy(e => e.Entity);
                case "IsRequired":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.IsRequired)
                        : queryable.OrderBy(e => e.IsRequired);
                case "Enum":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Enum)
                        : queryable.OrderBy(e => e.Enum);
                case "IsSystem":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.IsSystem)
                        : queryable.OrderBy(e => e.IsSystem);
                case "":
                case null:
                    return queryable.OrderBy(e => e.Id);
                default:
                    throw new ArgumentException("Property not found", nameof(propertyName));
            }
        }

        public static IQueryable<Field> Filter(this IQueryable<Field> queryable, FieldFilter filter)
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
            if (filter?.SizeFrom != null)
            {
                queryable = queryable.Where(e => e.Size >= filter.SizeFrom);
            }
            if (filter?.SizeTo != null)
            {
                queryable = queryable.Where(e => e.Size <= filter.SizeTo);
            }
            if (filter?.EntityId != null)
            {
                queryable = queryable.Where(e => e.EntityId == filter.EntityId);
            }
            if (filter?.IsRequired != null)
            {
                queryable = queryable.Where(e => e.IsRequired == filter.IsRequired);
            }
            if (filter?.EnumId != null)
            {
                queryable = queryable.Where(e => e.EnumId == filter.EnumId);
            }
            if (filter?.IsSystem != null)
            {
                queryable = queryable.Where(e => e.IsSystem == filter.IsSystem);
            }
            return queryable;
        }
    }
}
