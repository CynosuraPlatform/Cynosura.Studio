using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Cynosura.Studio.Generator.Models;
using Cynosura.Studio.Generator.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Fields
{
    public class UpdateField : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public FieldType? Type { get; set; }

        public int? Size { get; set; }

        public Guid? EntityId { get; set; }

        public bool? IsRequired { get; set; }

        public Guid? EnumId { get; set; }

        public PropertyCollection Properties { get; set; }

        public bool? IsSystem { get; set; }
    }
}
