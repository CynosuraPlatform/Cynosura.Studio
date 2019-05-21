using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Generator.Models;
using Cynosura.Studio.Core.Infrastructure;
using MediatR;
using Cynosura.Studio.Generator.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Fields
{
    public class CreateField : IRequest<CreatedEntity<Guid>>
    {
        public Guid Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
        public FieldType? Type { get; set; }
        [DisplayName("Size")]
        public int? Size { get; set; }
        [DisplayName("Entity")]
        public Guid? EntityId { get; set; }
        [DisplayName("Required")]
        public bool IsRequired { get; set; }
        [DisplayName("Enum")]
        public Guid? EnumId { get; set; }
        public PropertyCollection Properties { get; set; }
        [DisplayName("System")]
        public bool IsSystem { get; set; }
    }
}
