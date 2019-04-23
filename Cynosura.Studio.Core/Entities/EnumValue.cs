using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cynosura.Studio.Core.Entities
{
    public class EnumValue
    {
        [Required()]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(100)]
        public string DisplayName { get; set; }
        

        public int? Value { get; set; }
        
        [Required()]
        public Guid Id { get; set; }
        
        [Required()]
        public Guid EnumId { get; set; }
        public Enum Enum { get; set; }

        public Dictionary<string, object> Properties { get; set; }
    }
}
