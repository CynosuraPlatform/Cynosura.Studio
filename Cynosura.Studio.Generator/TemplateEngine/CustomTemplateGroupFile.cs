using System;
using System.IO;
using Antlr4.StringTemplate;

namespace Cynosura.Studio.Generator.TemplateEngine
{
    public class CustomTemplateGroupFile : TemplateGroupFile
    {
        public CustomTemplateGroupFile(string fileName) : base(fileName, '$', '$')
        {
        }

        public override Uri RootDirUri => new Uri($"file://{new FileInfo(FileName).Directory.FullName}");
    }
}
