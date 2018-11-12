using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class Entity : ISimpleTemplateProcessor
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

        [JsonIgnore]
        public Field DefaultField
        {
            get
            {
                var nameField = Fields.FirstOrDefault(f => f.Name == "Name");
                if (nameField != null)
                    return nameField;
                var stringField = Fields.FirstOrDefault(f => f.Type == FieldType.String);
                if (stringField != null)
                    return stringField;
                return Fields.FirstOrDefault();
            }
        }

        [JsonIgnore]
        public IList<Field> EntityFields
        {
            get
            {
                return Fields.Where(f => f.EntityId != null)
                    .ToList();
            }
        }

        [JsonIgnore]
        public IList<Entity> DependentEntities
        {
            get
            {
                return EntityFields.Select(f => f.Entity)
                    .Where(e => e.Id != Id)
                    .Distinct()
                    .ToList();
            }
        }

        [JsonIgnore]
        public IList<Field> EnumFields
        {
            get
            {
                return Fields.Where(f => f.EnumId != null)
                    .ToList();
            }
        }

        [JsonIgnore]
        public IList<Enum> DependentEnums
        {
            get
            {
                return EnumFields.Select(f => f.Enum)
                    .Distinct()
                    .ToList();
            }
        }

        public string ProcessTemplate(string template)
        {
            template = template.Replace("{Name}", Name);
            template = template.Replace("{PluralName}", PluralName);
            template = template.Replace("{NameLower}", NameLower);
            template = template.Replace("{PluralNameLower}", PluralNameLower);
            return template;
        }
    }
}
