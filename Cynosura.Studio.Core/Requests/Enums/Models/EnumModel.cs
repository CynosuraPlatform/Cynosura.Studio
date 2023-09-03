using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cynosura.Studio.Core.Requests.EnumValues.Models;

namespace Cynosura.Studio.Core.Requests.Enums.Models
{
    public class EnumModel
    {
        public EnumModel(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        [DisplayName("Id")]
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        public IList<EnumValueModel> Values { get; set; }

        public Dictionary<string, bool?> Properties { get; set; }
    }
}
