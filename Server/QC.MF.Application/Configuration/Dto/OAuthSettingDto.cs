using Abp.Zero.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration.Dto
{
    [Tab("第三方登录设置", 3)]
    public class OAuthSettingDto : ISettingDto
    {
        [Title("网站设置")]
        [DisplayName("网站首页地址")]
        [Key(AppSettingNames.SiteUrl)]
        public string SiteUrl { get; set; }
        
        [Title("微信")]
        [DisplayName("是否启用")]
        [Key(AppSettingNames.OAuth.WeixinOpen.IsEnabled)]
        public bool WeixinOpenIsEnabled { get; set; }
        [DisplayName("AppID")]
        [Key(AppSettingNames.OAuth.WeixinOpen.AppID)]
        public string WeixinOpenAppID { get; set; }
        [DisplayName("AppSecret")]
        [Key(AppSettingNames.OAuth.WeixinOpen.AppSecret)]
        public string WeixinOpenAppSecret { get; set; }


        [Title("支付宝")]
        [DisplayName("是否启用")]
        [Key(AppSettingNames.OAuth.Alipay.IsEnabled)]
        public bool AlipayIsEnabled { get; set; }
        [DisplayName("AppID")]
        [Key(AppSettingNames.OAuth.Alipay.AppID)]
        public string AlipayAppID { get; set; }
        [DisplayName("本地私钥")]
        [Key(AppSettingNames.OAuth.Alipay.AppPrivateKey)]
        public string AlipayAppPrivateKey { get; set; }
        [DisplayName("阿里公钥")]
        [Key(AppSettingNames.OAuth.Alipay.AlipayPublicKey)]
        public string AlipayAlipayPublicKey { get; set; }


        [Title("QQ")]
        [DisplayName("是否启用")]
        [Key(AppSettingNames.OAuth.QQ.IsEnabled)]
        public bool QQIsEnabled { get; set; }
        [DisplayName("AppID")]
        [Key(AppSettingNames.OAuth.QQ.AppID)]
        public string QQAppID { get; set; }
        [DisplayName("AppKey")]
        [Key(AppSettingNames.OAuth.QQ.AppKey)]
        public string QQAppKey { get; set; }


        [Title("微博")]
        [DisplayName("是否启用")]
        [Key(AppSettingNames.OAuth.Weibo.IsEnabled)]
        public bool WeiboIsEnabled { get; set; }
        [DisplayName("AppID")]
        [Key(AppSettingNames.OAuth.Weibo.AppID)]
        public string WeiboAppID { get; set; }
        [DisplayName("AppKey")]
        [Key(AppSettingNames.OAuth.Weibo.AppSecret)]
        public string WeibAppSecret { get; set; }
    }
}
