using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Infrastructure;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class CreateView : IRequest<CreatedEntity<Guid>>
    {
        public int SolutionId { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
