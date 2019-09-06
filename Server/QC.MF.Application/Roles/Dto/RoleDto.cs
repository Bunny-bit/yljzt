using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using QC.MF.Authorization.Roles;

namespace QC.MF.Roles.Dto
{
    /// <summary>
    /// 角色实体
    /// </summary>
    [AutoMapFrom(typeof(Role)), AutoMapTo(typeof(Role))]
    public class RoleDto : EntityDto<int>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        [StringLength(AbpRoleBase.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 角色显示名称
        /// </summary>
        [Required]
        [StringLength(AbpRoleBase.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// 是否系统角色
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 是否默认角色
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 权值列表
        /// </summary>
        public List<string> Permissions { get; set; }
    }
}
