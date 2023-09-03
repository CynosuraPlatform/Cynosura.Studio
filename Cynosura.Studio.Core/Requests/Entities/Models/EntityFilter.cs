using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Entities.Models
{
    public class EntityFilter : Infrastructure.EntityFilter
    {
        public string? Name { get; set; }
        public string? PluralName { get; set; }
        public string? DisplayName { get; set; }
        public string? PluralDisplayName { get; set; }
        public bool? IsAbstract { get; set; }
        public Guid? BaseEntityId { get; set; }
    }
}
