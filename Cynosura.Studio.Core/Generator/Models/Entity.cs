using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class Entity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PluralName { get; set; }
        public string DisplayName { get; set; }
        public string PluralDisplayName { get; set; }
        public IList<Field> Fields { get; set; }

        [JsonIgnore]
        public string NameLower => Name.ToLowerCamelCase();

        [JsonIgnore]
        public string PluralNameLower => PluralName.ToLowerCamelCase();
    }
}
