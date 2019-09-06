using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QC.MF.Api.Models
{
    public enum ThirdParty
    {
        QQ = 1,
        Weixin = 2,
        Weibo = 3,
        Alipay = 4,
    }

    public static class ThirdPartyExtension
    {
        public static string GetDescription(this ThirdParty value)
        {
            switch (value)
            {
                case ThirdParty.QQ:
                    return "QQ";
                case ThirdParty.Weixin:
                    return "微信";
                case ThirdParty.Weibo:
                    return "微博";
                case ThirdParty.Alipay:
                    return "支付宝";
                default:
                    return "未知平台";
            }
        }
    }

}
