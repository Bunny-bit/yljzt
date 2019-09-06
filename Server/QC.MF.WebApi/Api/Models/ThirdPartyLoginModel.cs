using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QC.MF.Api.Models
{
    /// <summary>
    /// 第三方登录请求参数
    /// </summary>
    public class ThirdPartyLoginModel
    {
        /// <summary>
        /// 登录平台
        /// </summary>
        public ThirdParty ThirdParty { get; set; }
        /// <summary>
        /// 登录 Authorization Code
        /// </summary>
        public string Code { get; set; }
    }
}
