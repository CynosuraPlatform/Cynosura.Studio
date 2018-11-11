using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Core.Generator.Models
{
    public interface ISimpleTemplateProcessor
    {
        string ProcessTemplate(string template);
    }
}
