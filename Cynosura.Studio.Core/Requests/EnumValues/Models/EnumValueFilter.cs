using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.EnumValues.Models
{
    public class EnumValueFilter : EntityFilter
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public int? ValueFrom { get; set; }
        public int? ValueTo { get; set; }
        public Guid? EnumId { get; set; }
    }
}
