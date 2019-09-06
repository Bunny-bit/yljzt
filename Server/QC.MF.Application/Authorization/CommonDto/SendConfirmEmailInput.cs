using System.ComponentModel.DataAnnotations;

namespace QC.MF.Authorization.Dto
{
    /// <summary>
    /// 发送激活账号邮件
    /// </summary>
    public class SendConfirmEmailInput
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
