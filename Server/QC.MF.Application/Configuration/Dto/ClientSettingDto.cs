using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration.Dto
{
    public class ClientSettingDto
    {
        /// <summary>
        /// 允许用户注册
        /// </summary>
        public bool AllowSelfRegistration { get; set; }
        /// <summary>
        /// 启用微信登录
        /// </summary>
        public bool WeixinOpenIsEnabled { get; set; }
        /// <summary>
        /// 启用支付宝登录
        /// </summary>
        public bool AlipayIsEnabled { get; set; }
        /// <summary>
        /// 启用QQ登录
        /// </summary>
        public bool QQIsEnabled { get; set; }
        /// <summary>
        /// 启用微博登录
        /// </summary>
        public bool WeiboIsEnabled { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }
    }
}
