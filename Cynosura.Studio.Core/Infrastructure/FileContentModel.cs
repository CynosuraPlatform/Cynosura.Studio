using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Core.Infrastructure
{
    public class FileContentModel
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
