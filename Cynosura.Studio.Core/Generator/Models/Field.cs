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
        public FieldType? Type { get; set; }
        public int? Size { get; set; }
        public Guid? EntityId { get; set; }
        public bool IsRequired { get; set; }
        public Guid? EnumId { get; set; }

        [JsonIgnore]
        public Entity Entity { get; set; }

        [JsonIgnore]
        public Enum Enum { get; set; }

        [JsonIgnore]
        public Type NetType
        {
            get
            {
                if (Type != null)
                    return FieldTypeInfo.Types[Type.Value].NetType;
                return null;
            }
        }

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
                if (Type != null)
                {
                    var typeName = FieldTypeInfo.Types[Type.Value].NetTypeName;
                    if (NetType.IsValueType && !IsRequired)
                        typeName += "?";
                    return typeName;
                }
                return null;
            }
        }

        [JsonIgnore]
        public string TypeNameNullable
        {
            get
            {
                if (Type != null)
                {
                    var typeName = FieldTypeInfo.Types[Type.Value].NetTypeName;
                    if (NetType.IsValueType)
                        typeName += "?";
                    return typeName;
                }
                return null;
            }
        }

        [JsonIgnore]
        public string JsTypeName
        {
            get
            {
                if (Type != null)
                    return FieldTypeInfo.Types[Type.Value].JsTypeName;
                return null;
            }
        }

        [JsonIgnore]
        public string EntityIdTypeName
        {
            get
            {
                if (Entity?.IdField.Type != null)
                {
                    var typeInfo = FieldTypeInfo.Types[Entity.IdField.Type.Value];
                    var typeName = typeInfo.NetTypeName;
                    if (typeInfo.NetType.IsValueType && !IsRequired)
                        typeName += "?";
                    return typeName;
                }
                return null;
            }
        }

        [JsonIgnore]
        public string Template {
            get
            {
                if (EntityId != null)
                    return "Entity";
                if (EnumId != null)
                    return "Enum";
                return "Type";
            }
        }

        [JsonIgnore]
        public string TypeTemplate => System.Enum.GetName(typeof(FieldType), Type);
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
        Guid,
        Blob
    }
}
