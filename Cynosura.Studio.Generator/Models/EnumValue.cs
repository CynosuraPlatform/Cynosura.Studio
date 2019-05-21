using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Generator.Infrastructure;
using Newtonsoft.Json;

namespace Cynosura.Studio.Generator.Models
{
    public class EnumValue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? Value { get; set; }
        public PropertyCollection Properties { get; set; }

        [JsonIgnore]
        public string DisplayNameOrName => !string.IsNullOrEmpty(DisplayName) ? DisplayName : Name;
    }
}
