using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 创建组织机构参数
    /// </summary>
    public class CreateOrganizationUnitInput
    {
        /// <summary>
        /// 上级机构ID
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; } 
    }
}
