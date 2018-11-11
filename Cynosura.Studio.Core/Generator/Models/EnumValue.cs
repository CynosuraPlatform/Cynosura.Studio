using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class EnumValue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? Value { get; set; }
        public Guid EnumId { get; set; }
        [JsonIgnore]
        public Enum Enum { get; set; }
    }
}
