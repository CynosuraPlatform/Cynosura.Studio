using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Enums.Models
{
    public class EnumFilter : EntityFilter
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
