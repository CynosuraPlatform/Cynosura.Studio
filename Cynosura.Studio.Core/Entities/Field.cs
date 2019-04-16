using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Cynosura.Studio.Core.Generator.Models;

namespace Cynosura.Studio.Core.Entities
{
    public class Field : BaseEntity
    {
        public Guid Id { get; set; }

        [Required()]
        [StringLength(100)]
        public string Name { get; set; }

        [Required()]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [Required()]
        public FieldType? Type { get; set; }

        public int? Size { get; set; }

        public Guid? EntityId { get; set; }

        public Entity Entity { get; set; }

        [Required()]
        public bool IsRequired { get; set; }
        
        public Guid? EnumId { get; set; }

        public Enum Enum { get; set; }

        public Dictionary<string, object> Properties { get; set; }
    }
}
