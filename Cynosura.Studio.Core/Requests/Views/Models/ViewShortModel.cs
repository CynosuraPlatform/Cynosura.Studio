using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Core.Requests.Views.Models
{
    public class ViewShortModel
    {
        public ViewShortModel(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
