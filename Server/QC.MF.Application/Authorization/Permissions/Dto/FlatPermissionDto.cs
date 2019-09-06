using Abp.AutoMapper;

namespace QC.MF.Authorization.Permissions.Dto
{
    /// <summary>
    /// 权限
    /// </summary>
    [AutoMapFrom(typeof(Abp.Authorization.Permission))]
    public class FlatPermissionDto
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
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsGrantedByDefault { get; set; }
    }
}
