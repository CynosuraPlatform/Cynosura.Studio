using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class CreateSolution : IRequest<int>
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
