using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class FieldTypeInfo
    {
        public static IDictionary<FieldType, FieldTypeInfo> Types { get; } = new Dictionary<FieldType, FieldTypeInfo>()
        {
            { FieldType.String, new FieldTypeInfo() { NetType = typeof(string), ShortTypeName = "string", JsTypeName = "string"} },
            { FieldType.Int32, new FieldTypeInfo() { NetType = typeof(int), ShortTypeName = "int", JsTypeName = "number"} },
            { FieldType.Int64, new FieldTypeInfo() { NetType = typeof(long), ShortTypeName = "long", JsTypeName = "number"} },
            { FieldType.Decimal, new FieldTypeInfo() { NetType = typeof(decimal), ShortTypeName = "decimal", JsTypeName = "number"} },
            { FieldType.Double, new FieldTypeInfo() { NetType = typeof(double), ShortTypeName = "double", JsTypeName = "number"} },
            { FieldType.Boolean, new FieldTypeInfo() { NetType = typeof(bool), ShortTypeName = "bool", JsTypeName = "boolean"} },
            { FieldType.DateTime, new FieldTypeInfo() { NetType = typeof(DateTime), ShortTypeName = "DateTime", JsTypeName = "Date"} },
            { FieldType.Date, new FieldTypeInfo() { NetType = typeof(DateTime), ShortTypeName = "DateTime", JsTypeName = "Date"} },
            { FieldType.Time, new FieldTypeInfo() { NetType = typeof(TimeSpan), ShortTypeName = "TimeSpan", JsTypeName = "string"} },
            { FieldType.Guid, new FieldTypeInfo() { NetType = typeof(Guid), ShortTypeName = "Guid", JsTypeName = "string"} },
        };

        public Type NetType { get; set; }
        public string JsTypeName { get; set; }
        public string ShortTypeName { get; set; }
    }
}
