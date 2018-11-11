using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class UpdateEnum : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
