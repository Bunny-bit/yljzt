using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Application.Services;
using QC.MF.Menus.Dto;

namespace QC.MF.Menus
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMenuAppService : IApplicationService
    {
        /// <summary>
        /// 创建用户自定义菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<MenuDto> CreateCustomMenu(CreateMenuInput input);

        /// <summary>
        /// 创建系统菜单，开发人员使用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<MenuDto> CreateSystemMenu(CreateMenuInput input);

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<MenuDto> UpdateMenu(UpdateMenuInput input);

        /// <summary>
        /// 移动菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<MenuDto> MoveMenu(MoveMenuInput input);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task DeleteMenu(DeleteMenuInput input);

        /// <summary>
        /// 获取菜单及权限
        /// </summary>
        /// <returns></returns>
        Task<IList<UserMenuItem>> GetUserMenus();

        /// <summary>
        /// 获取菜单及权限
        /// </summary>
        /// <returns></returns>
        Task<IList<MenuDto>> GetAllMenus();
    }
}
