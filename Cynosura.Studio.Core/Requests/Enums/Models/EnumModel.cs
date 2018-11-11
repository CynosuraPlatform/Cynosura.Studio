using System;
using System.Collections.Generic;
using Cynosura.Studio.Core.Requests.EnumValues.Models;

namespace Cynosura.Studio.Core.Requests.Enums.Models
{
    public class EnumModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IList<EnumValueModel> Values { get; set; }
    }
}
