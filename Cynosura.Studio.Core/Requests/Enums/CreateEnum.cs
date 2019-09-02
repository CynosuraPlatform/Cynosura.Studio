using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.EnumValues;
using Cynosura.Studio.Generator.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class CreateEnum : IRequest<CreatedEntity<Guid>>
    {
        public int SolutionId { get; set; }

        [DisplayName("Name")]
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z0-9]{2,}$", ErrorMessage = "Invalid name format")]
        public string Name { get; set; }
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
        public IList<CreateEnumValue> Values { get; set; }
        public PropertyCollection Properties { get; set; }
    }
}
