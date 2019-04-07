using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class UpdateEnumValue : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
        [DisplayName("Value")]
        public int? Value { get; set; }
    }
}
