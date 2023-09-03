using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class UpdateSolution : IRequest
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Path { get; set; }
        
        public string? TemplateName { get; set; }
        
        public string? TemplateVersion { get; set; }
    }
}
