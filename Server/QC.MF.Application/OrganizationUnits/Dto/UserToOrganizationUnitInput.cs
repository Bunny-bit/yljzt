using System.ComponentModel.DataAnnotations;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 用户加入组织请求
    /// </summary>
    public class UserToOrganizationUnitInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 组织机构ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}
