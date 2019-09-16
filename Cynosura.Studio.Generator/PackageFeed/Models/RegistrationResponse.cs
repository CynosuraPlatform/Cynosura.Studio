using System.Collections.Generic;

namespace Cynosura.Studio.Generator.PackageFeed.Models
{
    public class RegistrationResponse
    {
        public int Count { get; set; }
        public IEnumerable<RegistrationData> Items { get; set; }
    }
}