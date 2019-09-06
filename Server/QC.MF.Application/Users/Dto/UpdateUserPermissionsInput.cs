using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace QC.MF.Users.Dto
{
    /// <summary>
    /// 修改用户权限参数
    /// </summary>
    public class UpdateUserPermissionsInput 
    {

        [Range(1, int.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// 权限集合
        /// </summary>
        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}
