using System.ComponentModel.DataAnnotations;

namespace QC.MF.Authorization.Actives.Dto
{
    /// <summary>
    /// 邮箱地址验证
    /// </summary>
    public class ConfirmEmailInput
    {
        /// <summary>
        /// 密钥
        /// </summary>
        [Required]
        public string SecretKey { get; set; }
    }
}
