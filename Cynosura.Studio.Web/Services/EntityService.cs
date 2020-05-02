using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Entities;
using Cynosura.Studio.Core.Requests.Entities.Models;
using Cynosura.Studio.Web.Protos;
using Cynosura.Studio.Web.Protos.Entities;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

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
            return _mapper.Map<PageModel<EntityModel>, EntityPageModel>(await _mediator.Send(getEntities));
        }

        public override async Task<Entity> GetEntity(GetEntityRequest getEntityRequest, ServerCallContext context)
        {
            var getEntity = _mapper.Map<GetEntityRequest, GetEntity>(getEntityRequest);
            return _mapper.Map<EntityModel, Entity>(await _mediator.Send(getEntity));
        }

        [Authorize("WriteEntity")]
        public override async Task<Empty> UpdateEntity(UpdateEntityRequest updateEntityRequest, ServerCallContext context)
        {
            var updateEntity = _mapper.Map<UpdateEntityRequest, UpdateEntity>(updateEntityRequest);
            return _mapper.Map<Unit, Empty>(await _mediator.Send(updateEntity));
        }

        [Authorize("WriteEntity")]
        public override async Task<CreatedEntity> CreateEntity(CreateEntityRequest createEntityRequest, ServerCallContext context)
        {
            var createEntity = _mapper.Map<CreateEntityRequest, CreateEntity>(createEntityRequest);
            return _mapper.Map<CreatedEntity<Guid>, CreatedEntity>(await _mediator.Send(createEntity));
        }

        [Authorize("WriteEntity")]
        public override async Task<Empty> DeleteEntity(DeleteEntityRequest deleteEntityRequest, ServerCallContext context)
        {
            var deleteEntity = _mapper.Map<DeleteEntityRequest, DeleteEntity>(deleteEntityRequest);
            return _mapper.Map<Unit, Empty>(await _mediator.Send(deleteEntity));
        }
    }
}
