using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Core.Requests.FileGroups.Models
{
    public class FileGroupShortModel
    {
        public FileGroupShortModel(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
