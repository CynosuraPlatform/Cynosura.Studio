using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class CreateSolution : IRequest<int>
    {
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Path")]
        public string Path { get; set; }
    }
}
