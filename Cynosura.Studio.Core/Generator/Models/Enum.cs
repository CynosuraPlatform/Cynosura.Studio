﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class Enum : ISimpleTemplateProcessor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IList<EnumValue> Values { get; set; }

        [JsonIgnore]
        public string NameLower => Name.ToLowerCamelCase();

        public string ProcessTemplate(string template)
        {
            template = template.Replace("{Name}", Name);
            template = template.Replace("{NameLower}", NameLower);
            return template;
        }
    }
}