using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class CreateRole : IRequest<CreatedEntity<int>>
    {
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
