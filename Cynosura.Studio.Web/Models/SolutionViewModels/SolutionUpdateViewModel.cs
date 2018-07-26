using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cynosura.Studio.Web.Models.SolutionViewModels
{
    public class SolutionUpdateViewModel
    {
		[Required()]
		[StringLength(50)]
		public string Name { get; set; }[Required()]
		[StringLength(200)]
		public string Path { get; set; }
    }
}
