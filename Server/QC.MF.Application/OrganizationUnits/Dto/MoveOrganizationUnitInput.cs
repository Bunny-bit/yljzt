using System.ComponentModel.DataAnnotations;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 移动组织机构请求
    /// </summary>
    public class MoveOrganizationUnitInput
    {
        /// <summary>
        /// 组织机构ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// 新的上级组织机构ID
        /// </summary>
        public long? NewParentId { get; set; }
    }
}
