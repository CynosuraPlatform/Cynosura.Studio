using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cynosura.Studio.Generator.Models
{
    public class FieldAttribute
    {
        public Type Type { get; set; } = null!;
        public IList<object> Parameters { get; set; } = null!;
        public string Name => Regex.Replace(Type.Name, "Attribute$", "");
    }
}
