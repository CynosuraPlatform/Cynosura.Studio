using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class CreateSolution : IRequest<CreatedEntity<int>>
    {
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Path")]
        public string Path { get; set; }
        [DisplayName("Template Name")]
        public string TemplateName { get; set; }
        [DisplayName("Template Version")]
        public string TemplateVersion { get; set; }
    }
}
