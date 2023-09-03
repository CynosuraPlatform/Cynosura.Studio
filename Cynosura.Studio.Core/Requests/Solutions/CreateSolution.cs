using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class CreateSolution : IRequest<CreatedEntity<int>>
    {
        public string? Name { get; set; }

        public string? Path { get; set; }

        public string? TemplateName { get; set; }
        
        public string? TemplateVersion { get; set; }
    }
}
