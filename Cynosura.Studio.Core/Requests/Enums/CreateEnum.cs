using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.EnumValues;
using Cynosura.Studio.Generator.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class CreateEnum : IRequest<CreatedEntity<Guid>>
    {
        public int SolutionId { get; set; }

        public string? Name { get; set; }

        public string? DisplayName { get; set; }

        public IList<CreateEnumValue> Values { get; set; }

        public PropertyCollection Properties { get; set; }
    }
}
