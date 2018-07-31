using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Core.Services.Models
{
    public class EntityUpdateModel
    {
		public string Name { get; set; }
		public string PluralName { get; set; }
		public string DisplayName { get; set; }
		public string PluralDisplayName { get; set; }
        public IList<FieldUpdateModel> Fields { get; set; }
    }
}
