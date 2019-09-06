using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Abp.Web.Models;
using Abp.WebApi.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace QC.MF.Api.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenusController : ApiController
    {
        private readonly IUserNavigationManager _userNavigationManager;
        public IAbpSession AbpSession { get; set; }
        public MenusController(
            IUserNavigationManager userNavigationManager)
        {
            _userNavigationManager = userNavigationManager;
        }
        /// <summary>
        /// 获取菜单及权限
        /// </summary>
        /// <returns></returns>
        [AbpApiAuthorize]
        public async Task<AjaxResponse> GetMenus()
        {
            return new AjaxResponse((await _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier())).Items);
        }
    }
}
