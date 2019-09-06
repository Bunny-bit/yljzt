using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using QC.MF.Authorization.Users;
using Abp.Runtime.Validation;
using Abp.Extensions;

namespace QC.MF.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class UpdateCurrentUserInput : IShouldNormalize
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }


        /// <summary>
        /// 姓氏    【可不使用】
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [StringLength(User.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        public void Normalize()
        {
            if (Surname.IsNullOrWhiteSpace())
            {
                Surname = Name;
            }
        }
    }
}
