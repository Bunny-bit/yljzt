using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.UI;
using QC.MF.Authorization.Permissions.Dto;

namespace QC.MF.Authorization.Permissions
{

    [AbpAuthorize]
    public class PermissionAppService : MFAppServiceBase, IPermissionAppService
    {
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        public ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();
            var rootPermissions = permissions.Where(p => p.Parent == null);

            var result = new List<FlatPermissionWithLevelDto>();

            foreach (var rootPermission in rootPermissions)
            {
                var level = 0;
                AddPermission(rootPermission, permissions, result, level);
            }

            return new ListResultDto<FlatPermissionWithLevelDto>
            {
                Items = result
            };
        }

        private void AddPermission(Permission permission, IReadOnlyList<Permission> allPermissions, List<FlatPermissionWithLevelDto> result, int level)
        {
            var flatPermission = permission.MapTo<FlatPermissionWithLevelDto>();
            flatPermission.Level = level;
            result.Add(flatPermission);

            if (permission.Children == null)
            {
                return;
            }

            var children = allPermissions.Where(p => p.Parent != null && p.Parent.Name == permission.Name).ToList();

            foreach (var childPermission in children)
            {
                AddPermission(childPermission, allPermissions, result, level + 1);
            }
        }

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <returns></returns>
        public List<PermissionDto> GetAllPermissionTree()
        {
            var roots = PermissionManager.GetAllPermissions().Where(p=>p.Parent  == null).ToList();
            return roots.MapTo<List<PermissionDto>>();
        }

        /// <inheritdoc />
        public async Task<List<string>> GetUserPermissions()
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录，未能获取用户权限");
            }
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var grantedPermissions = await UserManager.GetGrantedPermissionsAsync(user);
            return grantedPermissions.Select(p => p.Name).ToList();
        }
    }
}
