using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 更新组织机构信息请求
    /// </summary>
    public class UpdateOrganizationUnitInput
    {
        /// <summary>
        /// 组织机构ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
    }
}
