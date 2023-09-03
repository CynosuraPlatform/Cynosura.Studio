using System;
using System.Linq;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.EnumValues.Models;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public static class EnumValueExtensions
    {
        public static IOrderedQueryable<EnumValue> OrderBy(this IQueryable<EnumValue> queryable, string? propertyName, OrderDirection? direction)
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
                case "Value":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Value)
                        : queryable.OrderBy(e => e.Value);
                case "Enum":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Enum.Name)
                        : queryable.OrderBy(e => e.Enum.Name);
                case "":
                case null:
                    return queryable.OrderBy(e => e.Id);
                default:
                    throw new ArgumentException("Property not found", nameof(propertyName));
            }
        }

        public static IQueryable<EnumValue> Filter(this IQueryable<EnumValue> queryable, EnumValueFilter? filter)
        {
            if (!string.IsNullOrEmpty(filter?.Text))
            {
                queryable = queryable.Where(e => e.Name!.Contains(filter.Text) || e.DisplayName!.Contains(filter.Text));
            }
            if (!string.IsNullOrEmpty(filter?.Name))
            {
                queryable = queryable.Where(e => e.Name!.Contains(filter.Name));
            }
            if (!string.IsNullOrEmpty(filter?.DisplayName))
            {
                queryable = queryable.Where(e => e.DisplayName!.Contains(filter.DisplayName));
            }
            if (filter?.ValueFrom != null)
            {
                queryable = queryable.Where(e => e.Value >= filter.ValueFrom);
            }
            if (filter?.ValueTo != null)
            {
                queryable = queryable.Where(e => e.Value <= filter.ValueTo);
            }
            if (filter?.EnumId != null)
            {
                queryable = queryable.Where(e => e.EnumId == filter.EnumId);
            }
            return queryable;
        }
    }
}
