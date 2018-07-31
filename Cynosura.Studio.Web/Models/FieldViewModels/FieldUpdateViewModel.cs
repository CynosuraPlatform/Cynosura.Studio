using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator.Models;

namespace Cynosura.Studio.Web.Models.FieldViewModels
{
    public class FieldUpdateViewModel
    {
        public Guid Id { get; set; }

        [Required()]
        [StringLength(100)]
        public string Name { get; set; }

        [Required()]
        [StringLength(100)]
        public string DisplayName { get; set; }

        public FieldType Type { get; set; }

        public int? Size { get; set; }

        [Required()]
        public bool IsRequired { get; set; }


    }
}
