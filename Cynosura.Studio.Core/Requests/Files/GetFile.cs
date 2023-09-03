using System;
using MediatR;
using Cynosura.Studio.Core.Requests.Files.Models;

namespace Cynosura.Studio.Core.Requests.Files
{
    public class GetFile : IRequest<FileModel?>
    {
        public int Id { get; set; }
    }
}
