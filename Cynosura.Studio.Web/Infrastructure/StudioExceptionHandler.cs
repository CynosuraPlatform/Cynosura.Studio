using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Infrastructure
{
    public class StudioExceptionHandler: IExceptionHandler
    {
        public ObjectResult HandleException(StudioException exception)
        {
            return new BadRequestObjectResult(new
            {
                Message = exception.Message,
                Error = exception.Message,
                ErrorCode = exception.Code,
                InnerError = exception.InnerException?.Message
            });
        }
            
        public ObjectResult HandleException(Exception exception)
        {
            return HandleException((StudioException) exception);
        }

        public Type ExceptionType { get; } = typeof(StudioException);
    }
}
