using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using QC.MF.Authorization.Roles;

namespace QC.MF.Roles.Dto
{
    /// <summary>
    /// 角色
    /// </summary>
    [AutoMapFrom(typeof(Role))]
    public class RoleListDto : EntityDto/*, IHasCreationTime*/
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否是系统内置角色
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 是否是默认角色
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime { get; set; }
    }
}
