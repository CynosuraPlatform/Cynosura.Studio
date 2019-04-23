using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cynosura.Studio.Core.Entities
{
    public class Solution : BaseEntity
    {
        [Required()]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required()]
        [StringLength(200)]
        public string Path { get; set; }
        
    }
}
