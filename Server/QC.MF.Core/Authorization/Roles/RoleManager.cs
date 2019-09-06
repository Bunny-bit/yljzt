using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.Zero;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;
using QC.MF.Authorization.Users;

namespace QC.MF.Authorization.Roles
{
    public class RoleManager : AbpRoleManager<Role, User>
    {
        public RoleManager(
            RoleStore store,
            IPermissionManager permissionManager,
            IRoleManagementConfig roleManagementConfig,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                store,
                permissionManager,
                roleManagementConfig,
                cacheManager,
                unitOfWorkManager)
        {
        }
    }
}
