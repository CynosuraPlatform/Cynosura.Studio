using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Fields;
using Cynosura.Studio.Generator.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class CreateEntity : IRequest<CreatedEntity<Guid>>
    {
        public int SolutionId { get; set; }
        [DisplayName("Name")]
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z0-9]{2,}$", ErrorMessage = "Invalid name format")]
        public string Name { get; set; }
        [DisplayName("Plural Name")]
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z0-9]{2,}$", ErrorMessage = "Invalid plural name format")]
        public string PluralName { get; set; }
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
        [DisplayName("Plural Display Name")]
        public string PluralDisplayName { get; set; }
        public IList<CreateField> Fields { get; set; }
        public PropertyCollection Properties { get; set; }
        [DisplayName("Abstract")]
        public bool IsAbstract { get; set; }
        [DisplayName("Base Entity")]
        public Guid? BaseEntityId { get; set; }
    }
}
