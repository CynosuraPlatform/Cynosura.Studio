using System;
using System.Collections.Generic;
using System.Linq;
using Cynosura.Studio.Generator.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cynosura.Studio.Generator.Models
{
    public class CodeTemplate
    {
        private IEnumerable<TemplateType> _types;

        public CodeTemplate()
        {
            Targets = new string[0];
        }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string TemplatePath { get; set; }
        [Obsolete]
        public TemplateType Type
        {
            get { return Types.FirstOrDefault(); }
            set { Types = new[] { value }; }
        }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<TemplateType> Types
        {
            get => _types;
            set
            {
                _types = value;
                _types = _types.Select(t =>
                {
#pragma warning disable CS0612 // Type or member is obsolete
                    if (t == TemplateType.View)
#pragma warning restore CS0612 // Type or member is obsolete
                    {
                        View = Models.View.DefaultViewName;
                        return TemplateType.Entity;
                    }
#pragma warning disable CS0612 // Type or member is obsolete
                    else if (t == TemplateType.EnumView)
#pragma warning restore CS0612 // Type or member is obsolete
                    {
                        View = Models.View.DefaultViewName;
                        return TemplateType.Enum;
                    }
                    else
                    {
                        return t;
                    }
                }).ToList();
            }
        }
        public string InsertAfter { get; set; }
        public IEnumerable<string> Targets { get; set; }
        public string View { get; set; }

        public bool ShouldSerializeType()
        {
            return false;
        }

        public bool CheckTargets(PropertyCollection properties)
        {
            if (Targets == null)
                return true;
            var targets = Targets.ToList();
            return targets.Count == 0 ||
                   targets.All(a => properties[a] is bool val && val);
        }

        public bool CheckTypes(IEnumerable<TemplateType> templateTypes)
        {
            if (templateTypes == null)
                return false;
            var templateTypeList = templateTypes.ToList();
            return Types.Any(t => templateTypeList.Contains(t));
        }

        public bool CheckView(View view)
        {
            if (view == null && string.IsNullOrEmpty(View))
            {
                return true;
            }
            if (view?.Name == View)
            {
                return true;
            }
            return false;
        }
    }

    public enum TemplateType
    {
        Entity,
        [Obsolete]
        View,
        Enum,
        [Obsolete]
        EnumView,
        AbstractEntity,
    }
}
