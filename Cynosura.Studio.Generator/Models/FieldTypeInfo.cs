using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Generator.Models
{
    public class FieldTypeInfo
    {
        public static IDictionary<FieldType, FieldTypeInfo> Types { get; } = new Dictionary<FieldType, FieldTypeInfo>()
        {
            { FieldType.String, new FieldTypeInfo() { NetType = typeof(string), NetTypeName = "string", JsTypeName = "string"} },
            { FieldType.Int32, new FieldTypeInfo() { NetType = typeof(int), NetTypeName = "int", JsTypeName = "number"} },
            { FieldType.Int64, new FieldTypeInfo() { NetType = typeof(long), NetTypeName = "long", JsTypeName = "number"} },
            { FieldType.Decimal, new FieldTypeInfo() { NetType = typeof(decimal), NetTypeName = "decimal", JsTypeName = "number"} },
            { FieldType.Double, new FieldTypeInfo() { NetType = typeof(double), NetTypeName = "double", JsTypeName = "number"} },
            { FieldType.Boolean, new FieldTypeInfo() { NetType = typeof(bool), NetTypeName = "bool", JsTypeName = "boolean"} },
            { FieldType.DateTime, new FieldTypeInfo() { NetType = typeof(DateTime), NetTypeName = "DateTime", JsTypeName = "Date"} },
            { FieldType.Date, new FieldTypeInfo() { NetType = typeof(DateTime), NetTypeName = "DateTime", JsTypeName = "Date"} },
            { FieldType.Time, new FieldTypeInfo() { NetType = typeof(TimeSpan), NetTypeName = "TimeSpan", JsTypeName = "string"} },
            { FieldType.Guid, new FieldTypeInfo() { NetType = typeof(Guid), NetTypeName = "Guid", JsTypeName = "string"} },
            { FieldType.Blob, new FieldTypeInfo() { NetType = typeof(byte[]), NetTypeName = "byte[]", JsTypeName = "Uint8Array"} },
        };

        public Type NetType { get; set; }
        public string JsTypeName { get; set; }
        public string NetTypeName { get; set; }
    }
}
