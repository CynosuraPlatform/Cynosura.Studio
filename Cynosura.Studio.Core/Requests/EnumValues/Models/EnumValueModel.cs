using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Core.Requests.EnumValues.Models
{
    public class EnumValueModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? Value { get; set; }
        public int EnumId { get; set; }

        public Enums.Models.EnumShortModel Enum { get; set; }
    }
}
