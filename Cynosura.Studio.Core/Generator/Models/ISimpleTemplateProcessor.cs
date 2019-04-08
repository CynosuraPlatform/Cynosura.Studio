using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Generator.Models
{
    public interface ISimpleTemplateProcessor
    {
        string ProcessTemplate(string template);
        PropertyCollection Properties { get; }
    }
}
