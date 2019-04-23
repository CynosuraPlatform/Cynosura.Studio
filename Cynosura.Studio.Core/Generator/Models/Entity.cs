using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cynosura.Studio.Core.Infrastructure;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class Entity : IGenerationObject
    {
        private string _nameKebab;
        private string _pluralNameKebab;

        public Entity()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PluralName { get; set; }
        public string DisplayName { get; set; }
        public string PluralDisplayName { get; set; }
        public bool IsAbstract { get; set; }
        public Guid? BaseEntityId { get; set; }
        public IList<Field> Fields { get; set; }
        public PropertyCollection Properties { get; set; }

        [JsonIgnore]
        public Entity BaseEntity { get; set; }

        [JsonIgnore]
        public Field IdField {
            get
            {
                var idField = AllSystemFields.FirstOrDefault(f => f.Name == "Id");
                if (idField == null)
                {
                    idField = new Field()
                    {
                        Name = "Id",
                        DisplayName = "Id",
                        IsRequired = true,
                        Type = FieldType.Int32,
                    };
                }
                return idField;
            }
        }

        [JsonIgnore]
        public IList<Field> AllFields
        {
            get
            {
                var allFields = new List<Field>();
                if (BaseEntity != null)
                    allFields.AddRange(BaseEntity.AllFields);
                allFields.AddRange(Fields.Where(f => !f.IsSystem));
                return allFields;
            }
        }

        [JsonIgnore]
        public IList<Field> AllSystemFields
        {
            get
            {
                var allFields = new List<Field>();
                if (BaseEntity != null)
                    allFields.AddRange(BaseEntity.AllSystemFields);
                allFields.AddRange(Fields.Where(f => f.IsSystem));
                return allFields;
            }
        }

        [JsonIgnore]
        public string NameLower => Name.ToLowerCamelCase();

        [JsonIgnore]
        public string PluralNameLower => PluralName.ToLowerCamelCase();

        [JsonIgnore]
        public string NameKebab => _nameKebab ?? (_nameKebab = Name.ToKebabCase());

        [JsonIgnore]
        public string PluralNameKebab => _pluralNameKebab ?? (_pluralNameKebab = PluralName.ToKebabCase());

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

        [JsonIgnore]
        public IDictionary<string, List<Field>> FieldsByType
        {
            get
            {
                return Fields
                    .GroupBy(f => f.Type)
                    .Where(g => g.Key != null)
                    .ToDictionary(g => g.Key.Value.ToString("g"), g => g.ToList());
            }
        }

        public string ProcessTemplate(string template)
        {
            template = template.Replace("{Name}", Name);
            template = template.Replace("{PluralName}", PluralName);
            template = template.Replace("{NameLower}", NameLower);
            template = template.Replace("{PluralNameLower}", PluralNameLower);
            template = template.Replace("{NameKebab}", NameKebab);
            template = template.Replace("{PluralNameKebab}", PluralNameKebab);
            return template;
        }

        public IEnumerable<TemplateType> GetTemplateTypes()
        {
            if (IsAbstract)
                return new[] { TemplateType.AbstractEntity };
            return new[] { TemplateType.Entity };
        }

        public IEnumerable<TemplateType> GetViewTemplateTypes()
        {
            if (IsAbstract)
                return new TemplateType[] { };
            return new[] { TemplateType.View };
        }
    }
}
