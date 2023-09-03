using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cynosura.Studio.Core.Entities
{
    public class View
    {
        public View(string name)
        {
            Name = name;
        }

        [Required()]
        public Guid Id { get; set; }
        
        [Required()]
        [StringLength(100)]
        public string Name { get; set; }
        
    }
}
