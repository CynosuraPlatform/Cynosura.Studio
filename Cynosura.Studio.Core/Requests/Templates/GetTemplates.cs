using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Core.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Templates
{
    public class GetTemplates: IRequest<IEnumerable<TemplateModel>>
    {
    }
}
