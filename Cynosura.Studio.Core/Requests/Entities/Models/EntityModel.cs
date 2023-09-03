using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cynosura.Studio.Core.Requests.Fields.Models;

namespace Cynosura.Studio.Core.Requests.Entities.Models
{
    public class EntityModel
    {
        public EntityModel(string name, string pluralName, string displayName, string pluralDisplayName, bool isAbstract)
        {
            Name = name;
            PluralName = pluralName;
            DisplayName = displayName;
            PluralDisplayName = pluralDisplayName;
            IsAbstract = isAbstract;
        }

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

        public IList<FieldModel> Fields { get; set; } = null!;

        public Dictionary<string, bool?> Properties { get; set; } = null!;

        [DisplayName("Abstract")]
        public bool IsAbstract { get; set; }

        [DisplayName("Base Entity")]
        public Guid? BaseEntityId { get; set; }
        public Entities.Models.EntityShortModel? BaseEntity { get; set; }
    }
}
