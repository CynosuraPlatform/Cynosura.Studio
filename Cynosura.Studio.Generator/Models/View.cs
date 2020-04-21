using System;

namespace Cynosura.Studio.Generator.Models
{
    public class View
    {
        public static string DefaultViewName { get; } = "Ng";

        public Guid Id { get; set; }

        public string Name { get; set; } = DefaultViewName;
    }
}
