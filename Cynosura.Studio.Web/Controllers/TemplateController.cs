using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Templates;
using Cynosura.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [ValidateModel]
    [Route("api")]
    public class TemplateController
    {
        private readonly IMediator _mediator;

        public TemplateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetTemplates")]
        public async Task<IEnumerable<TemplateModel>> GetTemplates()
        {
            return await _mediator.Send(new GetTemplates());
        }
    }
}
