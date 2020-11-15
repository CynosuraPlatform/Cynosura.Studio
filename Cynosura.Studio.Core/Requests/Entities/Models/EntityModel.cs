using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cynosura.Studio.Core.Requests.Fields.Models;

namespace Cynosura.Studio.Core.Requests.Entities.Models
{
    public class EntityModel
    {
        [DisplayName("Id")]
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Plural Name")]
        public string PluralName { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Plural Display Name")]
        public string PluralDisplayName { get; set; }

        public IList<FieldModel> Fields { get; set; }

        public Dictionary<string, bool?> Properties { get; set; }

        [DisplayName("Abstract")]
        public bool IsAbstract { get; set; }

        [DisplayName("Base Entity")]
        public Guid? BaseEntityId { get; set; }
        public Entities.Models.EntityShortModel BaseEntity { get; set; }
    }
}
