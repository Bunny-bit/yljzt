using System.ComponentModel.DataAnnotations;

namespace QC.MF.Authorization.Dto
{
    /// <summary>
    /// 邮箱地址验证
    /// </summary>
    public class ConfirmEmailCodeInput : SendConfirmEmailInput
    {
        /// <summary>
        /// 密钥
        /// </summary>
        [Required]
        public string Code { get; set; }
    }
}
