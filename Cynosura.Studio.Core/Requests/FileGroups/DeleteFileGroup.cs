using System;
using MediatR;

namespace Cynosura.Studio.Core.Requests.FileGroups
{
    public class DeleteFileGroup : IRequest
    {
        public int Id { get; set; }
    }
}
