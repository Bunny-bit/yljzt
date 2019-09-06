using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 组织机构信息
    /// </summary>
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class OrganizationUnitDto : AuditedEntityDto<long>
    {
        /// <summary>
        /// 上级机构ID
        /// </summary>
        public long? ParentId { get; set; }
        
        /// <summary>
        /// 机构Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 组织机构人员数量
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        /// 下级组织机构
        /// </summary>
        public List<OrganizationUnitDto> Children { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrganizationUnitDto()
        {
            Children = new List<OrganizationUnitDto>();
        }
    }
}
