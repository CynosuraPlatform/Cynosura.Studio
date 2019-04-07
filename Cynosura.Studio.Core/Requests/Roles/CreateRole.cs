using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class CreateRole : IRequest<int>
    {
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
