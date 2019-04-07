using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Requests.Fields;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class UpdateEntity : IRequest
    {
        public int SolutionId { get; set; }
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Plural Name")]
        public string PluralName { get; set; }
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
        [DisplayName("Plural Display Name")]
        public string PluralDisplayName { get; set; }
        public IList<UpdateField> Fields { get; set; }
    }
}
