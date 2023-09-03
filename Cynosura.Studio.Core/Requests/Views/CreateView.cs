using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class CreateView : IRequest<CreatedEntity<Guid>>
    {
        public int SolutionId { get; set; }

        public string? Name { get; set; }
    }
}
