using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Core.Requests.EnumValues.Models
{
    public class EnumValueModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? Value { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
