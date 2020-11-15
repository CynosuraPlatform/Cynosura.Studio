using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cynosura.Studio.Generator.Models;
using Cynosura.Studio.Core.Requests.Entities.Models;

namespace Cynosura.Studio.Core.Requests.Fields.Models
{
    public class FieldModel
    {
        [DisplayName("Id")]
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
        public FieldType? Type { get; set; }

        [DisplayName("Size")]
        public int? Size { get; set; }

        [DisplayName("Entity")]
        public Guid? EntityId { get; set; }
        public EntityShortModel Entity { get; set; }

        [DisplayName("Required")]
        public bool IsRequired { get; set; }

        [DisplayName("Enum")]
        public Guid? EnumId { get; set; }
        public Enums.Models.EnumShortModel Enum { get; set; }

        public Dictionary<string, bool?> Properties { get; set; }

        [DisplayName("System")]
        public bool IsSystem { get; set; }
    }
}
