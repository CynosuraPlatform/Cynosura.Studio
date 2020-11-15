using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Cynosura.Studio.Core.Requests.EnumValues.Models
{
    public class EnumValueModel
    {
        [DisplayName("Id")]
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Value")]
        public int? Value { get; set; }

        public Dictionary<string, bool?> Properties { get; set; }
    }
}
