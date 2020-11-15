using System;
using MediatR;
using Cynosura.Studio.Core.Requests.FileGroups.Models;

namespace Cynosura.Studio.Core.Requests.FileGroups
{
    public class GetFileGroup : IRequest<FileGroupModel>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
