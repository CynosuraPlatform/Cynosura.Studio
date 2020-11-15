using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Fields;
using Cynosura.Studio.Generator.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class CreateEntity : IRequest<CreatedEntity<Guid>>
    {
        public int SolutionId { get; set; }

        public string Name { get; set; }

        public string PluralName { get; set; }

        public string DisplayName { get; set; }

        public string PluralDisplayName { get; set; }

        public IList<CreateField> Fields { get; set; }

        public PropertyCollection Properties { get; set; }

        public bool? IsAbstract { get; set; }

        public Guid? BaseEntityId { get; set; }
    }
}
