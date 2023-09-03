using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class OpenSolution : IRequest<int>
    {
        [DisplayName("Path")]
        public string? Path { get; set; }
    }
}
