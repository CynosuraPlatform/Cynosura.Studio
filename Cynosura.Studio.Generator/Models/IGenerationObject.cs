using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Generator.Infrastructure;

namespace Cynosura.Studio.Generator.Models
{
    public interface IGenerationObject
    {
        string ProcessTemplate(string template);
        PropertyCollection Properties { get; }
    }
}
