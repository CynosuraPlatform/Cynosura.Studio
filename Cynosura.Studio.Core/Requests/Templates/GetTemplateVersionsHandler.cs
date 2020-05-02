using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.PackageFeed;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Templates
{
    public class GetTemplateVersionsHandler : IRequestHandler<GetTemplateVersions, IEnumerable<string>>
    {
        private readonly IPackageFeed _packageFeed;

        public GetTemplateVersionsHandler(IPackageFeed packageFeed)
        {
            _packageFeed = packageFeed;
        }

        public async Task<IEnumerable<string>> Handle(GetTemplateVersions request, CancellationToken cancellationToken)
        {
            return await _packageFeed.GetVersionsAsync(request.TemplateName);
        }
    }
}
