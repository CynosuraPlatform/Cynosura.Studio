using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Templates
{
    public class GetTemplateVersions : IRequest<IEnumerable<string>>
    {
        public string TemplateName { get; set; }
    }
}
