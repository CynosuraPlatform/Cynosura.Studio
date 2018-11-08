using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cynosura.Studio.Core.Requests.Fields;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class CreateEntity : IRequest<Guid>
    {
        public int SolutionId { get; set; }
        public string Name { get; set; }
        public string PluralName { get; set; }
        public string DisplayName { get; set; }
        public string PluralDisplayName { get; set; }
        public IList<CreateField> Fields { get; set; }
    }
}
