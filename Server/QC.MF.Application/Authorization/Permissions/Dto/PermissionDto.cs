using System.Collections.Generic;
using Abp.AutoMapper;

namespace QC.MF.Authorization.Permissions.Dto
{
    /// <summary>
    /// 权值
    /// </summary>
    [AutoMapFrom(typeof(Abp.Authorization.Permission))]
    public class PermissionDto
    {
        /// <summary>
        /// 上级名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 默认授予权限
        /// </summary>
        public bool IsGrantedByDefault { get; set; }
        /// <summary>
        /// 子权限
        /// </summary>
        public List<PermissionDto> Children { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PermissionDto()
        {
            Children = new List<PermissionDto>();
        }
    }
}
