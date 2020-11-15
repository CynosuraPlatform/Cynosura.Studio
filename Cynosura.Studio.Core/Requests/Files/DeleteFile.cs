using System;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Files
{
    public class DeleteFile : IRequest
    {
        public int Id { get; set; }
    }
}
