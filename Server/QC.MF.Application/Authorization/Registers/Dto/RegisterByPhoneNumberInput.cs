using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace QC.MF.Authorization.Registers.Dto
{
    /// <summary>
    /// 手机号注册
    /// </summary>
    public class RegisterByPhoneNumberInput
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
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  短信验证码
        /// </summary>
        public string Captcha { get; set; }

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
    }
}
