using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Core.Entities
{
    public class Field
    {
        public Field(string name, string displayName, bool isRequired, bool isSystem)
        {
            Name = name;
            DisplayName = displayName;
            IsRequired = isRequired;
            IsSystem = isSystem;
        }

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

        public Entity? Entity { get; set; }

        [Required()]
        public bool IsRequired { get; set; }
        
        [Required()]
        public bool IsSystem { get; set; }
        
        [Required()]
        public Guid Id { get; set; }
        
        public Guid? EnumId { get; set; }

        public Enum? Enum { get; set; }

        public Dictionary<string, bool?> Properties { get; set; }
    }
}
