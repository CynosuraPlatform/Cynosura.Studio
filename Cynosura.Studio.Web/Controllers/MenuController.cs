using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Studio.Core.Security;
using Cynosura.Studio.Web.Models.MenuViewModels;
using Cynosura.Web.Infrastructure.Menu;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [Route("api/[controller]")]
    public class MenuController
    {
        private readonly IMapper _mapper;
        private readonly IMenuProvider _menuProvider;

        public MenuController(IMapper mapper, IMenuProvider menuProvider)
        {
            _mapper = mapper;
            _menuProvider = menuProvider;
        }

        [HttpGet("")]
        public MenuViewModel GetMenu(UserInfoModel userInfo)
        {
            var menuItems = _menuProvider.GetMenuItems().Where(item =>
            {
                if (item.Roles == null || item.Roles.Count == 0)
                    return true;
                if (userInfo.Roles == null)
                    return false;
                return userInfo.Roles.Any(userRole => item.Roles.Contains(userRole));
            }).ToList();
            return new MenuViewModel()
            {
                Items = menuItems.Select(_mapper.Map<MenuItem, MenuItemViewModel>).ToList()
            };
        }

    }
}
