using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace QC.MF.Authorization.Registers.Dto
{
    /// <summary>
    /// 手机号注册(只有手机号)
    /// </summary>
    public class RegisterOnlyPhoneNumberInput
    {

        /// <summary>
        /// 手机号
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
    }
}
