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
using Cynosura.Studio.Core.Requests.Entities;
using Cynosura.Studio.Core.Requests.Entities.Models;
using Cynosura.Studio.Web.Protos;
using Cynosura.Studio.Web.Protos.Entities;

namespace Cynosura.Studio.Web.Services
{
    [Authorize("ReadEntity")]
    public class EntityService : Protos.Entities.EntityService.EntityServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EntityService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<EntityPageModel> GetEntities(GetEntitiesRequest getEntitiesRequest, ServerCallContext context)
        {
            var getEntities = _mapper.Map<GetEntitiesRequest, GetEntities>(getEntitiesRequest);
            var model = await _mediator.Send(getEntities);
            return _mapper.Map<PageModel<EntityModel>, EntityPageModel>(model);
        }

        public override async Task<Entity> GetEntity(GetEntityRequest getEntityRequest, ServerCallContext context)
        {
            var getEntity = _mapper.Map<GetEntityRequest, GetEntity>(getEntityRequest);
            var model = await _mediator.Send(getEntity);
            return _mapper.Map<EntityModel, Entity>(model!);
        }

        [Authorize("WriteEntity")]
        public override async Task<Empty> UpdateEntity(UpdateEntityRequest updateEntityRequest, ServerCallContext context)
        {
            var updateEntity = _mapper.Map<UpdateEntityRequest, UpdateEntity>(updateEntityRequest);
            var model = await _mediator.Send(updateEntity);
            return _mapper.Map<Unit, Empty>(model);
        }

        [Authorize("WriteEntity")]
        public override async Task<CreatedEntity> CreateEntity(CreateEntityRequest createEntityRequest, ServerCallContext context)
        {
            var createEntity = _mapper.Map<CreateEntityRequest, CreateEntity>(createEntityRequest);
            var model = await _mediator.Send(createEntity);
            return _mapper.Map<CreatedEntity<Guid>, CreatedEntity>(model);
        }

        [Authorize("WriteEntity")]
        public override async Task<Empty> DeleteEntity(DeleteEntityRequest deleteEntityRequest, ServerCallContext context)
        {
            var deleteEntity = _mapper.Map<DeleteEntityRequest, DeleteEntity>(deleteEntityRequest);
            var model = await _mediator.Send(deleteEntity);
            return _mapper.Map<Unit, Empty>(model);
        }
    }
}
