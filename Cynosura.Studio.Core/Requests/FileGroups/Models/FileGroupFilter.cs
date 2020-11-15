using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.FileGroups.Models
{
    public class FileGroupFilter : EntityFilter
    {
        public string Name { get; set; }
        public Core.Enums.FileGroupType? Type { get; set; }
        public string Location { get; set; }
        public string Accept { get; set; }
    }
}
