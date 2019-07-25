using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Templates
{
    public class GetTemplates: IRequest<IEnumerable<TemplateModel>>
    {
    }

    public class GetTemplatesHandler : IRequestHandler<GetTemplates, IEnumerable<TemplateModel>>
    {
        private readonly ITemplateProvider _templateProvider;

        public GetTemplatesHandler(ITemplateProvider templateProvider)
        {
            _templateProvider = templateProvider;
        }

        public Task<IEnumerable<TemplateModel>> Handle(GetTemplates request, CancellationToken cancellationToken)
        {
            return _templateProvider.GetTemplatesAsync();
        }
    }
}
