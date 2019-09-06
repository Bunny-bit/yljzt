using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using QC.MF.Validation;
using Abp.Extensions;

namespace QC.MF.Authorization.Registers.Dto
{
    /// <summary>
    /// 邮箱注册
    /// </summary>
    public class RegisterByEmailInput
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [DisableAuditing]
        public string Password { get; set; }

        /// <summary>
        /// 第三方登录令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///  邮件验证码
        /// </summary>
        public string Captcha { get; set; }
    }
}
