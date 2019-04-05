using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [Route("api/[controller]")]
    public class PropertiesController: ControllerBase
    {
        [HttpGet]
        public Dictionary<string, object> GetDefaults()
        {
            return PropertyCollection.Defaults;
        }
    }
}
