using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Requests.EnumValues;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class UpdateEnum : IRequest
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IList<UpdateEnumValue> Values { get; set; }
    }
}
