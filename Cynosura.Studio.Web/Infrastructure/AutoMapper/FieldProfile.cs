using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models.FieldViewModels;

namespace Cynosura.Studio.Web.Infrastructure.AutoMapper
{
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            CreateMap<Field, FieldViewModel>();
            CreateMap<FieldUpdateViewModel, FieldUpdateModel>();
        }
    }
}
