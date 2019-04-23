using System;
using System.Linq;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Users.Models;

namespace Cynosura.Studio.Core.Requests.Users
{
    public static class UserExtensions
    {
        public static IOrderedQueryable<User> OrderBy(this IQueryable<User> queryable, string propertyName, OrderDirection? direction)
        {
            switch (propertyName)
            {                
                case "UserName":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.UserName)
                        : queryable.OrderBy(e => e.UserName);
                case "Email":
                    return direction == OrderDirection.Descending
                        ? queryable.OrderByDescending(e => e.Email)
                        : queryable.OrderBy(e => e.Email);
                case "":
                case null:
                    return queryable.OrderBy(e => e.Id);
                default:
                    throw new ArgumentException("Property not found", nameof(propertyName));
            }
        }

        public static IQueryable<User> Filter(this IQueryable<User> queryable, UserFilter filter)
        {
            if (!string.IsNullOrEmpty(filter?.Text))
            {
                queryable = queryable.Where(e => e.UserName.Contains(filter.Text) || e.Email.Contains(filter.Text));
            }
            if (!string.IsNullOrEmpty(filter?.UserName))
            {
                queryable = queryable.Where(e => e.UserName.Contains(filter.UserName));
            }
            if (!string.IsNullOrEmpty(filter?.Email))
            {
                queryable = queryable.Where(e => e.Email.Contains(filter.Email));
            }
            return queryable;
        }
    }
}
