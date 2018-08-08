using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator.Models;

namespace Cynosura.Studio.Core.Services.Models
{
    public class FieldUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public FieldType? Type { get; set; }
        public int? Size { get; set; }
        public Guid? EntityId { get; set; }
        public bool IsRequired { get; set; }
    }
}
