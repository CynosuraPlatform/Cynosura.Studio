using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cynosura.Studio.Core.Entities
{
    public class Entity
    {
        public Entity(string name, string pluralName, string displayName, string pluralDisplayName)
        {
            Name = name;
            PluralName = pluralName;
            DisplayName = displayName;
            PluralDisplayName = pluralDisplayName;
        }

		[Required()]
		[StringLength(100)]
		public string Name { get; set; }

		[Required()]
		[StringLength(100)]
		public string PluralName { get; set; }

		[Required()]
		[StringLength(100)]
		public string DisplayName { get; set; }

		[Required()]
		[StringLength(100)]
		public string PluralDisplayName { get; set; }

        public IList<Field> Fields { get; set; } = null!;

        public Dictionary<string, bool?> Properties { get; set; } = null!;
        [Required()]
        public bool IsAbstract { get; set; }
        

        public Guid? BaseEntityId { get; set; }
        public Entity? BaseEntity { get; set; }
        
        [Required()]
        public Guid Id { get; set; }
        
    }
}
