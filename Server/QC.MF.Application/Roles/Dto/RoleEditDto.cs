using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using QC.MF.Authorization.Roles;

namespace QC.MF.Roles.Dto
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [AutoMap(typeof(Role))]
    public class RoleEditDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否是默认角色
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
