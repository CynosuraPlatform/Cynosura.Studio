using System.Collections.Generic;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class Entity
    {
        public string Name { get; set; }
        public string PluralName { get; set; }
        public string DisplayName { get; set; }
        public string PluralDisplayName { get; set; }
        public IList<Field> Fields { get; set; }

        public string NameLower => Name.ToLowerCamelCase();
        public string PluralNameLower => PluralName.ToLowerCamelCase();
    }
}
