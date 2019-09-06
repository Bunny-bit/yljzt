using System.ComponentModel.DataAnnotations;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 用户加入组织请求
    /// </summary>
    public class UsersToOrganizationUnitInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserIdListStr { get; set; }

        /// <summary>
        /// 组织机构ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}
