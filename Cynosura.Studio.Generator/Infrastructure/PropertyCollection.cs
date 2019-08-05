using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Generator.Infrastructure
{
    public class PropertyCollection : Dictionary<string, object>
    {
        public PropertyCollection() : base(StringComparer.InvariantCultureIgnoreCase)
        {

        }

        public static Dictionary<string, object> Defaults = new Dictionary<string, object>()
        {
            {PropertyNames.View, true},
            {PropertyNames.ViewList, true},
            {PropertyNames.ViewEdit, true},
            {PropertyNames.Core, true},
            {PropertyNames.Web, true},
            {PropertyNames.Data, true}
        };

        public new object this[string key]
        {
            get => (ContainsKey(key) ? base[key] : Defaults[key]) ?? new object();
            set => base[key] = value;
        }
    }
}
