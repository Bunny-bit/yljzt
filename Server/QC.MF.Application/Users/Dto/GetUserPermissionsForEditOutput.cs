using System.Collections.Generic;
using Abp.Application.Services.Dto;
using QC.MF.Authorization.Permissions.Dto;

namespace QC.MF.Users.Dto
{
    /// <summary>
    /// 用户权值
    /// </summary>
    public class GetUserPermissionsForEditOutput
    {
        /// <summary>
        /// 权值
        /// </summary>
        public List<PermissionDto> Permissions { get; set; }
        /// <summary>
        /// 已授予的权值
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}
