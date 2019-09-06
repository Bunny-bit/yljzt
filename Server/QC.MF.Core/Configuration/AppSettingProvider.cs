using System.Collections.Generic;
using Abp.Configuration;
using System.Configuration;
using Abp.Zero.Configuration;
using QC.MF.Security;
using System.Configuration;
using Abp.Json;

namespace QC.MF.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            context.Manager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled).DefaultValue = false.ToString().ToLowerInvariant();
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "#108ee9", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.Skin, "默认 ", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.SiteUrl, "http://mf.kingyuer.cn:666/index.html", scopes: SettingScopes.Application, isVisibleToClients: true),

                new SettingDefinition(AppSettingNames.UserManagement.AllowSelfRegistration, ConfigurationManager.AppSettings[AppSettingNames.UserManagement.AllowSelfRegistration] ?? "true"),
                new SettingDefinition(AppSettingNames.UserManagement.IsNewRegisteredUserActiveByDefault, ConfigurationManager.AppSettings[AppSettingNames.UserManagement.IsNewRegisteredUserActiveByDefault] ?? "false"),
                new SettingDefinition(AppSettingNames.UserManagement.UseCaptchaOnRegistration, ConfigurationManager.AppSettings[AppSettingNames.UserManagement.UseCaptchaOnRegistration] ?? "true"),
                new SettingDefinition(AppSettingNames.UserManagement.IsPhoneNumberConfirmationRequiredForLogin, ConfigurationManager.AppSettings[AppSettingNames.UserManagement.IsPhoneNumberConfirmationRequiredForLogin] ?? "false"),


                new SettingDefinition(AppSettingNames.TenantManagement.AllowSelfRegistration,ConfigurationManager.AppSettings[AppSettingNames.TenantManagement.UseCaptchaOnRegistration] ?? "true"),
                new SettingDefinition(AppSettingNames.TenantManagement.IsNewRegisteredTenantActiveByDefault,ConfigurationManager.AppSettings[AppSettingNames.TenantManagement.IsNewRegisteredTenantActiveByDefault] ??"false"),
                new SettingDefinition(AppSettingNames.TenantManagement.UseCaptchaOnRegistration,ConfigurationManager.AppSettings[AppSettingNames.TenantManagement.UseCaptchaOnRegistration] ?? "true"),
                new SettingDefinition(AppSettingNames.TenantManagement.DefaultEdition,ConfigurationManager.AppSettings[AppSettingNames.TenantManagement.DefaultEdition] ?? ""),
                new SettingDefinition(AppSettingNames.Security.PasswordComplexity, PasswordComplexitySetting.DefaultPasswordComplexitySetting.ToJsonString()),



                new SettingDefinition(AppSettingNames.SMS.DefaultSender, ConfigurationManager.AppSettings[AppSettingNames.SMS.DefaultSender] ?? "QCSMSSenderManager"),
                new SettingDefinition(AppSettingNames.SMS.FreeSignName, ConfigurationManager.AppSettings[AppSettingNames.SMS.FreeSignName] ?? "青才科技"),
                new SettingDefinition(AppSettingNames.SMS.Ali.RegionEndpoint, ConfigurationManager.AppSettings[AppSettingNames.SMS.Ali.RegionEndpoint] ?? "http://1910484995838290.mns.cn-shenzhen.aliyuncs.com/"),
                new SettingDefinition(AppSettingNames.SMS.Ali.AccessKeyId, ConfigurationManager.AppSettings[AppSettingNames.SMS.Ali.AccessKeyId] ?? "LTAIJQTANmluFsyt"),
                new SettingDefinition(AppSettingNames.SMS.Ali.SecretAccessKey, ConfigurationManager.AppSettings[AppSettingNames.SMS.Ali.SecretAccessKey] ?? "5CXwsmA7aOQbK7S2cGwgwsViPXRSwU"),
                new SettingDefinition(AppSettingNames.SMS.Ali.TopicName, ConfigurationManager.AppSettings[AppSettingNames.SMS.Ali.TopicName] ?? "sms.topic-cn-shenzhen"),
                new SettingDefinition(AppSettingNames.SMS.Ali.TemplateCode, ConfigurationManager.AppSettings[AppSettingNames.SMS.Ali.TemplateCode] ?? "SMS_57610033"),

                new SettingDefinition(AppSettingNames.SMS.QC.Url, ConfigurationManager.AppSettings[AppSettingNames.SMS.QC.Url] ?? "http://sms.easyitcn.cn/sms"),
                new SettingDefinition(AppSettingNames.SMS.QC.Username, ConfigurationManager.AppSettings[AppSettingNames.SMS.QC.Username] ?? "zth"),
                new SettingDefinition(AppSettingNames.SMS.QC.Password, ConfigurationManager.AppSettings[AppSettingNames.SMS.QC.Password] ?? "123456"),


                new SettingDefinition(AppSettingNames.OAuth.WeixinOpen.IsEnabled, ConfigurationManager.AppSettings[AppSettingNames.OAuth.WeixinOpen.IsEnabled] ?? "false"),
                new SettingDefinition(AppSettingNames.OAuth.WeixinOpen.AppID, ConfigurationManager.AppSettings[AppSettingNames.OAuth.WeixinOpen.AppID] ?? ""),
                new SettingDefinition(AppSettingNames.OAuth.WeixinOpen.AppSecret, ConfigurationManager.AppSettings[AppSettingNames.OAuth.WeixinOpen.AppSecret] ?? ""),

                new SettingDefinition(AppSettingNames.OAuth.Alipay.IsEnabled, ConfigurationManager.AppSettings[AppSettingNames.OAuth.Alipay.IsEnabled] ?? "false"),
                new SettingDefinition(AppSettingNames.OAuth.Alipay.AppID, ConfigurationManager.AppSettings[AppSettingNames.OAuth.Alipay.AppID] ?? ""),
                new SettingDefinition(AppSettingNames.OAuth.Alipay.AppPrivateKey, ConfigurationManager.AppSettings[AppSettingNames.OAuth.Alipay.AppPrivateKey] ?? ""),
                new SettingDefinition(AppSettingNames.OAuth.Alipay.AlipayPublicKey, ConfigurationManager.AppSettings[AppSettingNames.OAuth.Alipay.AlipayPublicKey] ?? ""),

                new SettingDefinition(AppSettingNames.OAuth.QQ.IsEnabled, ConfigurationManager.AppSettings[AppSettingNames.OAuth.QQ.IsEnabled] ?? "false"),
                new SettingDefinition(AppSettingNames.OAuth.QQ.AppID, ConfigurationManager.AppSettings[AppSettingNames.OAuth.QQ.AppID] ?? ""),
                new SettingDefinition(AppSettingNames.OAuth.QQ.AppKey, ConfigurationManager.AppSettings[AppSettingNames.OAuth.QQ.AppKey] ?? ""),

                new SettingDefinition(AppSettingNames.OAuth.Weibo.IsEnabled, ConfigurationManager.AppSettings[AppSettingNames.OAuth.Weibo.IsEnabled] ?? "false"),
                new SettingDefinition(AppSettingNames.OAuth.Weibo.AppID, ConfigurationManager.AppSettings[AppSettingNames.OAuth.Weibo.AppID] ?? ""),
                new SettingDefinition(AppSettingNames.OAuth.Weibo.AppSecret, ConfigurationManager.AppSettings[AppSettingNames.OAuth.Weibo.AppSecret] ?? ""),

                new SettingDefinition(AppSettingNames.Captcha.Geetest.PublicKey, ConfigurationManager.AppSettings[AppSettingNames.Captcha.Geetest.PublicKey] ?? "7a444abca9308c3d4ff09593553b6783"),
                new SettingDefinition(AppSettingNames.Captcha.Geetest.PrivateKey, ConfigurationManager.AppSettings[AppSettingNames.Captcha.Geetest.PrivateKey] ?? "8de52190dec3ae0841b398bf683302d8"),


                new SettingDefinition(AppSettingNames.System.ImageUploadPath, ConfigurationManager.AppSettings[AppSettingNames.System.ImageUploadPath] ?? "~/Common/Images/UserPics"),
                new SettingDefinition(AppSettingNames.System.SystemName, ConfigurationManager.AppSettings[AppSettingNames.System.SystemName] ?? "后台管理系统",scopes: SettingScopes.Application | SettingScopes.Tenant),

                new SettingDefinition(AppSettingNames.Push.JPush.JPushAppKey, ConfigurationManager.AppSettings[AppSettingNames.Push.JPush.JPushAppKey] ?? ""),
                new SettingDefinition(AppSettingNames.Push.JPush.JPushMasterSecret, ConfigurationManager.AppSettings[AppSettingNames.Push.JPush.JPushMasterSecret] ?? ""),

                new SettingDefinition(AppSettingNames.Push.Getui.AppID, ConfigurationManager.AppSettings[AppSettingNames.Push.Getui.AppID] ?? ""),
                new SettingDefinition(AppSettingNames.Push.Getui.AppKey, ConfigurationManager.AppSettings[AppSettingNames.Push.Getui.AppKey] ?? ""),
                new SettingDefinition(AppSettingNames.Push.Getui.MasterSecret, ConfigurationManager.AppSettings[AppSettingNames.Push.Getui.MasterSecret] ?? ""),

            };
        }
    }
}
