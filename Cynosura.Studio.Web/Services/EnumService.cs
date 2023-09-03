using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Enums;
using Cynosura.Studio.Core.Requests.Enums.Models;
using Cynosura.Studio.Web.Protos;
using Cynosura.Studio.Web.Protos.Enums;

namespace Cynosura.Studio.Web.Services
{
    [Authorize("ReadEnum")]
    public class EnumService : Protos.Enums.EnumService.EnumServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EnumService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<EnumPageModel> GetEnums(GetEnumsRequest getEnumsRequest, ServerCallContext context)
        {
            var getEnums = _mapper.Map<GetEnumsRequest, GetEnums>(getEnumsRequest);
            var model = await _mediator.Send(getEnums);
            return _mapper.Map<PageModel<EnumModel>, EnumPageModel>(model);
        }

        public override async Task<Protos.Enums.Enum> GetEnum(GetEnumRequest getEnumRequest, ServerCallContext context)
        {
            var getEnum = _mapper.Map<GetEnumRequest, GetEnum>(getEnumRequest);
            var model = await _mediator.Send(getEnum);
            return _mapper.Map<EnumModel, Protos.Enums.Enum>(model!);
        }

        [Authorize("WriteEnum")]
        public override async Task<Empty> UpdateEnum(UpdateEnumRequest updateEnumRequest, ServerCallContext context)
        {
            var updateEnum = _mapper.Map<UpdateEnumRequest, UpdateEnum>(updateEnumRequest);
            var model = await _mediator.Send(updateEnum);
            return _mapper.Map<Unit, Empty>(model);
        }

        [Authorize("WriteEnum")]
        public override async Task<CreatedEntity> CreateEnum(CreateEnumRequest createEnumRequest, ServerCallContext context)
        {
            var createEnum = _mapper.Map<CreateEnumRequest, CreateEnum>(createEnumRequest);
            var model = await _mediator.Send(createEnum);
            return _mapper.Map<CreatedEntity<Guid>, CreatedEntity>(model);
        }

        [Authorize("WriteEnum")]
        public override async Task<Empty> DeleteEnum(DeleteEnumRequest deleteEnumRequest, ServerCallContext context)
        {
            var deleteEnum = _mapper.Map<DeleteEnumRequest, DeleteEnum>(deleteEnumRequest);
            var model = await _mediator.Send(deleteEnum);
            return _mapper.Map<Unit, Empty>(model);
        }
    }
}
