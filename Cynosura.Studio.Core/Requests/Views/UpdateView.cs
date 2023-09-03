using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class UpdateView : IRequest
    {
        public int SolutionId { get; set; }

        public Guid Id { get; set; }

        public string? Name { get; set; }
    }
}
