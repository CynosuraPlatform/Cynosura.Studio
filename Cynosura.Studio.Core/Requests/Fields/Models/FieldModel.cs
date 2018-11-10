using System;
using System.Collections.Generic;
using Cynosura.Studio.Core.Generator.Models;
using Cynosura.Studio.Core.Requests.Entities.Models;

namespace Cynosura.Studio.Core.Requests.Fields.Models
{
    public class FieldModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public FieldType? Type { get; set; }
        public int? Size { get; set; }
        public Guid? EntityId { get; set; }
        public EntityShortModel Entity { get; set; }
        public bool IsRequired { get; set; }
    }
}
