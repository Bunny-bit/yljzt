using System.Collections.Generic;
using Abp.Application.Services.Dto;
using QC.MF.Authorization.Permissions.Dto;

namespace QC.MF.Roles.Dto
{
    /// <summary>
    /// 编辑角色 需要的信息
    /// </summary>
    public class GetRoleForEditOutput
    {
        /// <summary>
        /// 角色
        /// </summary>
        public RoleEditDto Role { get; set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        public List<FlatPermissionDto> Permissions { get; set; }

        /// <summary>
        /// 该角色拥有的权限
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}
