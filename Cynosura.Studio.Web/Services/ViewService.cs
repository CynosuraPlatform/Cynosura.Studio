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
using Cynosura.Studio.Core.Requests.Views;
using Cynosura.Studio.Core.Requests.Views.Models;
using Cynosura.Studio.Web.Protos;
using Cynosura.Studio.Web.Protos.Views;

namespace Cynosura.Studio.Web.Services
{
    [Authorize("ReadView")]
    public class ViewService : Protos.Views.ViewService.ViewServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ViewService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<ViewPageModel> GetViews(GetViewsRequest getViewsRequest, ServerCallContext context)
        {
            var getViews = _mapper.Map<GetViewsRequest, GetViews>(getViewsRequest);
            var model = await _mediator.Send(getViews);
            return _mapper.Map<PageModel<ViewModel>, ViewPageModel>(model);
        }

        public override async Task<View> GetView(GetViewRequest getViewRequest, ServerCallContext context)
        {
            var getView = _mapper.Map<GetViewRequest, GetView>(getViewRequest);
            var model = await _mediator.Send(getView);
            return _mapper.Map<ViewModel, View>(model!);
        }

        [Authorize("WriteView")]
        public override async Task<Empty> UpdateView(UpdateViewRequest updateViewRequest, ServerCallContext context)
        {
            var updateView = _mapper.Map<UpdateViewRequest, UpdateView>(updateViewRequest);
            var model = await _mediator.Send(updateView);
            return _mapper.Map<Unit, Empty>(model);
        }

        [Authorize("WriteView")]
        public override async Task<CreatedEntity> CreateView(CreateViewRequest createViewRequest, ServerCallContext context)
        {
            var createView = _mapper.Map<CreateViewRequest, CreateView>(createViewRequest);
            var model = await _mediator.Send(createView);
            return _mapper.Map<CreatedEntity<Guid>, CreatedEntity>(model);
        }

        [Authorize("WriteView")]
        public override async Task<Empty> DeleteView(DeleteViewRequest deleteViewRequest, ServerCallContext context)
        {
            var deleteView = _mapper.Map<DeleteViewRequest, DeleteView>(deleteViewRequest);
            var model = await _mediator.Send(deleteView);
            return _mapper.Map<Unit, Empty>(model);
        }
    }
}
