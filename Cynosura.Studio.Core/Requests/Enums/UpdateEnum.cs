using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Requests.EnumValues;
using Cynosura.Studio.Generator.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class UpdateEnum : IRequest
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
        public IList<UpdateEnumValue> Values { get; set; }
        public PropertyCollection Properties { get; set; }
    }
}
