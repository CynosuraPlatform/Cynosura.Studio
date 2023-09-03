using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Core.Requests.Enums.Models
{
    public class EnumShortModel
    {
        public EnumShortModel(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
