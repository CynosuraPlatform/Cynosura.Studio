using System;
using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Files.Models;

namespace Cynosura.Studio.Core.Requests.Files
{
    public class DownloadFile : IRequest<FileContentModel>
    {
        public int Id { get; set; }
    }
}
