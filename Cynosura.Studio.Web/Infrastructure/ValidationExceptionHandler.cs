using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Infrastructure
{
    public class ValidationExceptionHandler : IExceptionHandler
    {
        public Type ExceptionType => typeof(ValidationException);

        public ObjectResult HandleException(Exception exception)
        {
            var validationException = (ValidationException)exception;
            return new BadRequestObjectResult(new
            {
                Message = validationException.Message,
                ModelState = GetModelState(validationException.Failures),
            });
        }

        private Dictionary<string, SimpleModelState> GetModelState(IDictionary<string, string[]> failures)
        {
            return failures.Select(p => new
                {
                    Property = p.Key,
                    Errors = p.Value.Select(e => new ModelStateError() { ErrorMessage = e })
                }).ToDictionary(p => p.Property, p => new SimpleModelState()
                {
                    Errors = p.Errors.ToList(),
                });
        }
    }
}
