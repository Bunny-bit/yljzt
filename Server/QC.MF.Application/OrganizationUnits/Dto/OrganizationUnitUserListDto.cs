using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using QC.MF.Authorization.Users;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [AutoMapFrom(typeof(User))]
    public class OrganizationUnitUserListDto : EntityDto<long>
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  显示名称
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 头像ID
        /// </summary>
        public Guid? ProfilePictureId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string AddedTime { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
