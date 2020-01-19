using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Account;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Register, User>();
        }
    }
}
