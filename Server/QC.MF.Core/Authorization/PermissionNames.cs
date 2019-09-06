namespace QC.MF.Authorization
{
    public static class PermissionNames
    {

        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";
        public const string Pages_Administration_Roles_Lookup = "Pages.Administration.Roles.Lookup";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_ChangeUserPassword = "Pages.Administration.Users.ChangeUserPassword";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";
        public const string Pages_Administration_Users_Active = "Pages.Administration.Users.Active";
        public const string Pages_Administration_Users_Lookup = "Pages.Administration.Users.Lookup";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";
        /// <summary>
        /// 管理组织机构权限
        /// </summary>
        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        /// <summary>
        /// 管理组织机构树权限
        /// </summary>
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_Lookup = "Pages.Administration.OrganizationUnits.Lookup";
        /// <summary>
        /// 管理组织机构内成员
        /// </summary>
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        /// <summary>
        /// hangfire后台面板
        /// </summary>
        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";
        /// <summary>
        /// 管理菜单权限
        /// </summary>
        public const string Pages_Administration_Menus = "Pages.Administration.Menus";

        /// <summary>
        /// 管理设置权限
        /// </summary>
        public const string Pages_Administration_Settings = "Pages.Administration.Settings";
        /// <summary>
        /// 管理App启动页权限
        /// </summary>
        public const string Pages_Administration_AppStartPage = "Pages.Administration.AppStartPage";

        /// <summary>
        /// App版本控制权限
        /// </summary>
        public const string Pages_Administration_AppEdition = "Pages.Administration.AppEdition";

        /// <summary>
        /// DemoMange
        /// </summary>
        public const string Pages_DemoMange = "Pages.DemoMange";
        /// <summary>
        /// DemoMange
        /// </summary>
        public const string Pages_DemoMangeCreate = "Pages.DemoMangeCreate";
        /// <summary>
        /// DemoMange
        /// </summary>
        public const string Pages_DemoMangeDelete = "Pages.DemoMangeDelete";
        /// <summary>
        /// DemoMange
        /// </summary>
        public const string Pages_DemoMangeUpdate = "Pages.DemoMangeUpdate";

        public const string FileSettingDemoMange = "Pages.FileSettingDemoMange";
    }
}
