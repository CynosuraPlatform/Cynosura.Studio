using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class UpdateEnumValue : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? Value { get; set; }
        public PropertyCollection Properties { get; set; }
    }
}
