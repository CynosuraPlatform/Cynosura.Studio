using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Generator.Models;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Fields
{
    public class CreateField : IRequest<int>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public FieldType? Type { get; set; }
        public int? Size { get; set; }
        public Guid? EntityId { get; set; }
        public bool? IsRequired { get; set; }
        public Guid? EnumId { get; set; }
    }
}
