using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Files.Models
{
    public class FileFilter : EntityFilter
    {
        public string Name { get; set; }
        public string ContentType { get; set; }

        public string Url { get; set; }
        public int? GroupId { get; set; }
    }
}
