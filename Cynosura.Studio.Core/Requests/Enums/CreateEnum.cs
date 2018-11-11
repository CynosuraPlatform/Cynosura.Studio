using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Requests.EnumValues;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class CreateEnum : IRequest<Guid>
    {
        public int SolutionId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IList<CreateEnumValue> Values { get; set; }
    }
}
