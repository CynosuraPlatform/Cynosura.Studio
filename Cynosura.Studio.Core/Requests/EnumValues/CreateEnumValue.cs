using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Generator.Infrastructure;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class CreateEnumValue : IRequest
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? DisplayName { get; set; }

        public int? Value { get; set; }

        public PropertyCollection Properties { get; set; }
    }
}
