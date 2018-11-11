using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cynosura.Studio.Core.Entities
{
    public class Enum : BaseEntity
    {
        public int Id { get; set; }

        [Required()]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required()]
        [StringLength(100)]
        public string DisplayName { get; set; }
        
    }
}
