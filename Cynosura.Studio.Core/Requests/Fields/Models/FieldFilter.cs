using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Fields.Models
{
    public class FieldFilter : EntityFilter
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? SizeFrom { get; set; }
        public int? SizeTo { get; set; }
        public Guid? EntityId { get; set; }
        public bool? IsRequired { get; set; }
        public Guid? EnumId { get; set; }
        public bool? IsSystem { get; set; }
    }
}
