using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Generator.Infrastructure
{
    public class PropertyCollection : Dictionary<string, bool?>
    {
        public PropertyCollection() : base(StringComparer.InvariantCultureIgnoreCase)
        {

        }

        public static Dictionary<string, bool> Defaults = new Dictionary<string, bool>()
        {
            {PropertyNames.View, true},
            {PropertyNames.ViewList, true},
            {PropertyNames.ViewEdit, true},
            {PropertyNames.Core, true},
            {PropertyNames.Web, true},
            {PropertyNames.Data, true}
        };

        public new bool? this[string key]
        {
            get => (ContainsKey(key) ? base[key] : Defaults[key]) ?? null;
            set => base[key] = value;
        }
    }
}
