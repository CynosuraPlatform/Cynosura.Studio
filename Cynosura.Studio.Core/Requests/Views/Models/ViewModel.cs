using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Cynosura.Studio.Core.Requests.Views.Models
{
    public class ViewModel
    {
        [DisplayName("Id")]
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
