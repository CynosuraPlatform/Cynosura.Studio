using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class CreateEnumValue : IRequest<int>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? Value { get; set; }
        public int? EnumId { get; set; }
    }
}
