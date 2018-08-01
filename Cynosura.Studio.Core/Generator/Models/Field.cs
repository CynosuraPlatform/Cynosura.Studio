using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class Field
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public FieldType Type { get; set; }
        public int? Size { get; set; }
        public bool IsRequired { get; set; }

        [JsonIgnore]
        public Type NetType => FieldTypeInfo.Types[Type].NetType;

        [JsonIgnore]
        public IList<FieldAttribute> Attributes
        {
            get
            {
                var attributes = new List<FieldAttribute>();
                if (IsRequired)
                    attributes.Add(new FieldAttribute() { Type = typeof(RequiredAttribute) });
                if (Type == FieldType.String)
                {
                    if (Size != null)
                    {
                        attributes.Add(new FieldAttribute() { Type = typeof(StringLengthAttribute), Parameters = new List<object>() { Size }});
                    }
                }
                else if (Type == FieldType.Date)
                {
                    attributes.Add(new FieldAttribute() { Type = typeof(ColumnAttribute), Parameters = new List<object>() { "TypeName = \"date\"" } });
                }
                return attributes;
            }
        }

        [JsonIgnore]
        public string NameLower => Name.ToLowerCamelCase();

        [JsonIgnore]
        public string TypeName 
        {
            get
            {
                var typeName = ShortTypeName;
                if (NetType.IsValueType && !IsRequired)
                    typeName += "?";
                return typeName;
            }
        }

        [JsonIgnore]
        public string JsTypeName => FieldTypeInfo.Types[Type].JsTypeName;

        [JsonIgnore]
        private string ShortTypeName => FieldTypeInfo.Types[Type].ShortTypeName;

        [JsonIgnore]
        public string Template => Enum.GetName(typeof(FieldType), Type);
    }

    public enum FieldType
    {
        String,
        Int32,
        Int64,
        Decimal,
        Double,
        Boolean,
        DateTime,
        Date,
        Time,
        Guid
    }
}
