using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using QC.MF.Authorization;
using QC.MF.Menus.Dto;

namespace QC.MF.Menus
{
    /// <summary>
    /// 菜单接口服务
    /// </summary>
    public class MenuAppService : MFAppServiceBase,IMenuAppService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IUserNavigationManager _userNavigationManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuAppService(IRepository<Menu> menuRepository, IUserNavigationManager userNavigationManager)
        {
            _menuRepository = menuRepository;
            _userNavigationManager = userNavigationManager;
        }

        /// <summary>
        /// 创建用户自定义菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        [AbpAuthorize(PermissionNames.Pages_Administration_Menus)]
        public async Task<MenuDto> CreateCustomMenu(CreateMenuInput input)
        {
            await  CheckNameRepeat(input.DisplayName);
            var menu = input.MapTo<Menu>();
            menu.IsSystem = false;
            menu.Target = "_blank";
            _menuRepository.Insert(menu);
            await CurrentUnitOfWork.SaveChangesAsync();
            await RebuildMenu();
            return menu.MapTo<MenuDto>();
        }

        /// <summary>
        /// 创建系统菜单，开发人员使用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Menus)]
        public async Task<MenuDto> CreateSystemMenu(CreateMenuInput input)
        {
            var menu = input.MapTo<Menu>();
            menu.IsSystem = true;
            menu.Target = "_self";
            _menuRepository.Insert(menu);
            await CurrentUnitOfWork.SaveChangesAsync();
            await RebuildMenu();
            return menu.MapTo<MenuDto>();
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Menus)]
        public async Task<MenuDto> UpdateMenu(UpdateMenuInput input)
        {
            var menu = await _menuRepository.GetAsync(input.Id);
            await CheckNameRepeat(input.DisplayName, menu);
            if (menu.IsSystem && menu.Url != input.Url)
            {
                throw new UserFriendlyException("系统菜单链接不可修改");
            }
            menu.DisplayName = input.DisplayName;
            menu.Icon = input.Icon;
            menu.IsVisible = input.IsVisible;
            menu.RequiredPermissionName = input.RequiredPermissionName;
            if (!menu.IsSystem)
            {
                menu.Url = input.Url;
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            await RebuildMenu();
            return menu.MapTo<MenuDto>();
        }

        /// <summary>
        /// 移动菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Menus)]
        public async Task<MenuDto> MoveMenu(MoveMenuInput input)
        {
            var children = await GetAllChildren(input.Id);
            if (children.Contains(input.NewParentId))
            {
                throw new UserFriendlyException("检查到嵌套层级结构，保存失败");
            }
            var menu = await _menuRepository.GetAsync(input.Id);
            if (menu.ParentId != input.NewParentId)
            {
                var menus = await _menuRepository.GetAll()
                    .Where(m => m.ParentId == menu.ParentId && m.Id != menu.Id)
                    .OrderBy(m=>m.Order)
                    .ToListAsync();
                for (int index = 0; index < menus.Count; index++)
                {
                    menus[index].Order = index;
                }
            }
            menu.ParentId = input.NewParentId;

            {
                var menus = await _menuRepository.GetAll()
                    .Where(m => m.ParentId == input.NewParentId || m.Id == menu.Id)
                    .OrderBy(m => m.Order)
                    .ToListAsync();
                for (int index = 0; index < menus.Count; index++)
                {
                    if (menu.Id == menus[index].Id)
                    {
                        menus[index].Order = input.NewOrder;
                    }
                    else if (index < input.NewOrder)
                    {
                        menus[index].Order = index;
                    }
                    else
                    {
                        menus[index].Order = index + 1;
                    }
                }
            }
            
            await CurrentUnitOfWork.SaveChangesAsync();
            await RebuildMenu();
            return menu.MapTo<MenuDto>();
        }

        private async Task<List<int>> GetAllChildren(int menuId)
        {
            var menuList = (await _menuRepository.GetAllListAsync()).MapTo<IList<MenuDto>>();
            var result = new List<int>();
            var parentIds = new List<int> { menuId };
            List<int> children;
            do
            {
                children = menuList.Where(m => parentIds.Contains(m.ParentId)).Select(m => m.Id).ToList();
                parentIds = children;
                result.AddRange(children);
            } while (children.Count > 0);
            return result;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Menus)]
        public async Task DeleteMenu(DeleteMenuInput input)
        {
            var menu = await _menuRepository.GetAsync(input.Id);
            if (menu.IsSystem)
            {
                throw new UserFriendlyException("系统菜单不可删除");
            }
            await _menuRepository.DeleteAsync(input.Id);
            var menus = await _menuRepository.GetAll().Where(m => m.ParentId == input.Id).ToListAsync();
            foreach (var m in menus)
            {
                m.ParentId = 0;
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            await RebuildMenu();
        }

        private async Task RebuildMenu()
        {
            var provider = IocManager.Instance.Resolve<DBNavigationProvider>();
            provider.SetNavigation(null);
        }


        /// <summary>
        /// 获取菜单及权限
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<IList<UserMenuItem>> GetUserMenus()
        {
            var menu = await _userNavigationManager.GetMenuAsync(DBNavigationProvider.MenuGroupName, AbpSession.ToUserIdentifier());
            return menu.Items;
        }

        /// <summary>
        /// 获取菜单及权限
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Administration_Menus)]
        public async Task<IList<MenuDto>> GetAllMenus()
        {
            var menuList = (await _menuRepository.GetAllListAsync()).MapTo<IList<MenuDto>>();
            var result = new List<MenuDto>(menuList.OrderBy(s => s.Order).Where(i => i.ParentId==0));
            BuildMenuTree(ref menuList, result);
            return result;
        }


        private void BuildMenuTree(ref IList<MenuDto> source, List<MenuDto> parents)
        {
            source = source.Except(parents).ToList();
            foreach (var parent in parents)
            {
                var children = source.Where(s => s.ParentId == parent.Id).OrderBy(s=>s.Order).ToList();
                parent.Items = children;
                if (children.Count > 0)
                {
                    BuildMenuTree(ref source, children);
                }
            }
        }


        private async Task CheckNameRepeat(string menuName, Menu old=null)
        {
            if (old != null && old.DisplayName == menuName)
            {
                return;
            }
            if (await _menuRepository.GetAll().AnyAsync(x => x.DisplayName == menuName))
            {
                throw new Abp.UI.UserFriendlyException("菜单名已存在，请更换。");
            }
        }
    }
}
