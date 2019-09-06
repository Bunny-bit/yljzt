using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Api.Models
{
    /// <summary>
    /// 发送短信前需验证验证码
    /// </summary>
    public class PhoneWithCaptchaInput
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        [RegularExpressionAttribute(@"^1[34578]\d{9}$", ErrorMessage = "手机号格式错误")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }
    }
}
