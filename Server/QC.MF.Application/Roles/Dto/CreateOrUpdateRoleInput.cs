using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace QC.MF.Roles.Dto
{
    
    public class CreateOrUpdateRoleInput
    {
        /// <summary>
        /// 角色信息
        /// </summary>
        [Required]
        public RoleEditDto Role { get; set; }

        /// <summary>
        /// 拥有的权限
        /// </summary>
        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}
