using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cynosura.Studio.Core.Entities
{
    public class Enum
    {
        public Enum(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        [Required()]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required()]
        [StringLength(100)]
        public string DisplayName { get; set; }
        
        [Required()]
        public Guid Id { get; set; }
        
        public Dictionary<string, bool?> Properties { get; set; } = null!;
    }
}
