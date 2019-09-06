using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using QC.MF.Authorization.Users;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 归属于组织单元的人员信息
    /// </summary>
    [AutoMap(typeof(User))]
    public class OrganizationUnitUserDto : EntityDto<long>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 用户创建时间
        /// </summary>
        public string CreationTime { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EmailAddress { get; set; }

    }
}
