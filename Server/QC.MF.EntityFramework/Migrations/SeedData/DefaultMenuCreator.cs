using System.Collections.Generic;
using System.Linq;
using Abp.Localization;
using QC.MF.EntityFramework;
using QC.MF.Authorization;

namespace QC.MF.Migrations.SeedData
{
    public class DefaultMenuCreator
    {
        public static List<Menus.Menu> InitialMenus { get; private set; }
        private readonly MFDbContext _context;

        public DefaultMenuCreator(MFDbContext context)
        {
            _context = context;
        }
        static DefaultMenuCreator()
        {
            InitialMenus = new List<Menus.Menu>
            {
                new Menus.Menu
                {
                    DisplayName = "用户",
                    Icon = "user",
                    Order = 1,
                    IsVisible = true,
                    Target = "_self",
                    IsSystem = true,
                    Url = "/user",
                    RequiredPermissionName = PermissionNames.Pages_Administration_Users_Lookup
                },
                new Menus.Menu
                {
                    DisplayName = "角色",
                    Icon = "trademark",
                    Order = 2,
                    IsVisible = true,
                    Target = "_self",
                    IsSystem = true,
                    Url = "/role",
                    RequiredPermissionName = PermissionNames.Pages_Administration_Roles_Lookup
                },
                new Menus.Menu
                {
                    DisplayName = "组织机构",
                    Icon = "apple",
                    Order = 3,
                    IsVisible = true,
                    Target = "_self",
                    IsSystem = true,
                    Url = "/organization",
                    RequiredPermissionName = PermissionNames.Pages_Administration_OrganizationUnits_Lookup
                },
                new Menus.Menu
                {
                    DisplayName = "菜单",
                    Icon = "bars",
                    Order = 4,
                    IsVisible = true,
                    Target = "_self",
                    IsSystem = true,
                    Url = "/menu",
                    RequiredPermissionName = PermissionNames.Pages_Administration_Menus
                },
                new Menus.Menu
                {
                    DisplayName = "设置",
                    Icon = "setting",
                    Order = 5,
                    IsVisible = true,
                    Target = "_self",
                    IsSystem = true,
                    Url = "/configuration",
                    RequiredPermissionName = PermissionNames.Pages_Administration_Settings
                },
                new Menus.Menu
                {
                    DisplayName = "审计日志",
                    Icon = "solution",
                    Order = 6,
                    IsVisible = true,
                    Target = "_self",
                    IsSystem = true,
                    Url = "/auditLog",
                    RequiredPermissionName = PermissionNames.Pages_Administration_AuditLogs
                }
            };
        }

        public void Create()
        {
            AddMenuIfNotExists();
        }

        private void AddMenuIfNotExists()
        {
            foreach (var menu in InitialMenus)
            {
                if (_context.Menus.Any(l => l.Url == menu.Url))
                {
                    continue;
                }
                _context.Menus.Add(menu);
            }
            _context.SaveChanges();
        }
    }
}
