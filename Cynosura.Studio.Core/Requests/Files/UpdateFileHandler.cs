﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Cynosura.Core.Data;
using Cynosura.Core.Services;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.FileStorage;

namespace Cynosura.Studio.Core.Requests.Files
{
    public class UpdateFileHandler : IRequestHandler<UpdateFile>
    {
        private readonly IEntityRepository<Core.Entities.File> _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UpdateFileHandler(IEntityRepository<Core.Entities.File> fileRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileStorage fileStorage,
            IStringLocalizer<SharedResource> localizer)
        {
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(UpdateFile request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetEntities()
                .Include(e => e.Group)
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (file == null)
            {
                throw new ServiceException(_localizer["{0} {1} not found", _localizer["File"], request.Id]);
            }
            file.Group.Accept.Validate(request.Name, request.ContentType);
            _mapper.Map(request, file);
            if (file.Group.Type == Core.Enums.FileGroupType.Database)
            {
                file.Content = request.Content.ConvertToBytes();
            }
            else if (file.Group.Type == Core.Enums.FileGroupType.Storage)
            {
                var oldUrl = file.Url;
                var filename = Guid.NewGuid() + Path.GetExtension(request.Name);
                var filePath = $"{file.Group.Location}/{DateTime.Today:yyyy'/'MM}/{filename}";
                file.Url = await _fileStorage.SaveFileAsync(filePath, request.Content, request.ContentType);
                await _fileStorage.DeleteFileAsync(oldUrl);
            }
            else
            {
                throw new NotSupportedException($"Group type {file.Group.Type} not supported");
            }
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }

    }
}
