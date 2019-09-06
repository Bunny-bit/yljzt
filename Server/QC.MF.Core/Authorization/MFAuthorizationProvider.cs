using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace QC.MF.Authorization
{
    public class MFAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(PermissionNames.Pages) ?? context.CreatePermission(PermissionNames.Pages, L("Pages"));

            var administration = pages.CreateChildPermission(PermissionNames.Pages_Administration, L("Administration"));

            var users = administration.CreateChildPermission(PermissionNames.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_ChangeUserPassword, L("修改密码"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Unlock, L("解锁"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Active, L("启用禁用"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Lookup, L("查看用户"));

            var roles = administration.CreateChildPermission(PermissionNames.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Delete, L("DeletingRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Lookup, L("查看角色"));

            var organizationUnits = administration.CreateChildPermission(PermissionNames.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(PermissionNames.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(PermissionNames.Pages_Administration_OrganizationUnits_Lookup, L("查看组织机构"));

            var organizationmenus = administration.CreateChildPermission(PermissionNames.Pages_Administration_Menus, L("Menus"));

            administration.CreateChildPermission(PermissionNames.Pages_Administration_AppStartPage, L("管理App启动页"));

            administration.CreateChildPermission(PermissionNames.Pages_Administration_Settings, L("设置"));

            administration.CreateChildPermission(PermissionNames.Pages_Administration_AuditLogs, L("AuditLogs"));

            administration.CreateChildPermission(PermissionNames.Pages_Administration_AppEdition, L("App版本控制管理"));

            var demoManage = pages.CreateChildPermission(PermissionNames.Pages_DemoMange, L("CRUD Demo"));
            demoManage.CreateChildPermission(PermissionNames.Pages_DemoMangeCreate, L("添加"));
            demoManage.CreateChildPermission(PermissionNames.Pages_DemoMangeDelete, L("删除"));
            demoManage.CreateChildPermission(PermissionNames.Pages_DemoMangeUpdate, L("编辑"));
            pages.CreateChildPermission(PermissionNames.FileSettingDemoMange, L("GetSetDemo"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MFConsts.LocalizationSourceName);
        }
    }
}
