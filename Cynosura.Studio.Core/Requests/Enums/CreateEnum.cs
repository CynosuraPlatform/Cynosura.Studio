using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class CreateEnum : IRequest<int>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
