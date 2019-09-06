using System.ComponentModel.DataAnnotations;

namespace QC.MF.Authorization.RestPasswords.Dto
{
    /// <summary>
    /// 通过邮箱找回
    /// </summary>
    public class ResetPasswordByEmailInput:VerificationResetPasswordByEmailInput
    {
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
    /// <summary>
    /// 验证邮箱验证码是否正确
    /// </summary>
    public class VerificationResetPasswordByEmailInput
    {
        /// <summary>
        /// 邮箱号
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        public string VerificationCode { get; set; }
    }
}
