using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Cynosura.Studio.Core.Generator.Models
{
    public class Field
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Type Type { get; set; }
        public int? Size { get; set; }
        public bool IsRequired { get; set; }
        public IList<FieldAttribute> Attributes
        {
            get
            {
                var attributes = new List<FieldAttribute>();
                if (IsRequired)
                    attributes.Add(new FieldAttribute() { Type = typeof(RequiredAttribute) });
                if (Type == typeof(string))
                {
                    if (Size != null)
                    {
                        attributes.Add(new FieldAttribute() { Type = typeof(StringLengthAttribute), Parameters = new List<object>() { Size }});
                    }
                }
                return attributes;
            }
        }

        public string NameLower => Name.ToLowerCamelCase();

        public string TypeName 
        {
            get
            {
                var typeName = GetShortTypeName(Type);
                if (Type.IsValueType && !IsRequired)
                    typeName += "?";
                return typeName;
            }
        }

        public string JsTypeName
        {
            get
            {
                if (Type == typeof(string))
                    return "string";
                else if (Type == typeof(int))
                    return "number";
                else if (Type == typeof(decimal))
                    return "number";
                else if (Type == typeof(DateTime))
                    return "Date";
                else if (Type == typeof(bool))
                    return "boolean";
                return "any";
            }
        }

        private string GetShortTypeName(Type type)
        {
            if (type == typeof(string))
                return "string";
            else if (Type == typeof(decimal))
                return "decimal";
            else if (Type == typeof(int))
                return "int";
            else if (Type == typeof(bool))
                return "bool";
            return Type.Name;
        }
    }
}
