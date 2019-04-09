using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Core.Infrastructure;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class Enum : IGenerationObject
    {
        private string _nameKebab;
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IList<EnumValue> Values { get; set; }
        public PropertyCollection Properties { get; set; }

        [JsonIgnore]
        public string NameLower => Name.ToLowerCamelCase();

        [JsonIgnore]
        public string NameKebab => _nameKebab ?? (_nameKebab = Name.ToKebabCase());

        public string ProcessTemplate(string template)
        {
            template = template.Replace("{Name}", Name);
            template = template.Replace("{NameLower}", NameLower);
            template = template.Replace("{NameKebab}", NameKebab);
            return template;
        }
    }
}
