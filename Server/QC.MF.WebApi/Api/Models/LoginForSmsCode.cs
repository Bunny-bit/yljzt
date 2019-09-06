using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Api.Models
{
    /// <summary>
    /// 通过手机号短信 登录
    /// </summary>
    public class LoginForSmsCode
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string Code { get; set; }

    }
}
