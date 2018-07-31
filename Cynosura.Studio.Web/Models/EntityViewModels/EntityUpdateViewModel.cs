using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models.FieldViewModels;

namespace Cynosura.Studio.Web.Models.EntityViewModels
{
    public class EntityUpdateViewModel
    {
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

        public IList<FieldUpdateViewModel> Fields { get; set; }
    }
}
