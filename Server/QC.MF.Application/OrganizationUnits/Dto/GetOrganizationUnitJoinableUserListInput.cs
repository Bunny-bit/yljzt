using Abp.Application.Services.Dto;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 获取可加入某组织机构的人员请求
    /// </summary>
    public class GetOrganizationUnitJoinableUserListInput : PagedResultRequestDto
    {
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 按名称过滤
        /// </summary>
        public string Filter { get; set; }
    }
}
