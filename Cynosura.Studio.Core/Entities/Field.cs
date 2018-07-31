using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public FieldType Type { get; set; }

        public int? Size { get; set; }

        [Required()]
        public bool IsRequired { get; set; }

    }

    public enum FieldType
    {
        String,
        Int32,
        Int64,
        Decimal,
        Double,
        Boolean,
        DateTime,
        Date,
        Time,
        Guid
    }
}
