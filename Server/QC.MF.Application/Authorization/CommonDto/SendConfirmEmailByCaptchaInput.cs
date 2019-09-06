using System.ComponentModel.DataAnnotations;

namespace QC.MF.Authorization.Dto
{
    /// <summary>
    /// 发送激活账号邮件
    /// </summary>
    public class SendConfirmEmailByCaptchaInput : SendConfirmEmailInput
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }
    }
}
