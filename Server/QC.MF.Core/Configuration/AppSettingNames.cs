namespace QC.MF.Configuration
{
    public static class AppSettingNames
    {
        public const string UiTheme = "App.UiTheme";
        public const string Skin = "App.Skin";
        public const string SiteUrl = "App.SiteUrl";
        public static class General
        {
            //no setting yet
        }

        public static class System
        {
            public const string ImageUploadPath = "App.ImageUploadPath";
            public const string SystemName = "App.SystemName";
        }
        public static class TenantManagement
        {
            public const string AllowSelfRegistration = "App.TenantManagement.AllowSelfRegistration";
            public const string IsNewRegisteredTenantActiveByDefault = "App.TenantManagement.IsNewRegisteredTenantActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.TenantManagement.UseCaptchaOnRegistration";
            public const string DefaultEdition = "App.TenantManagement.DefaultEdition";
        }

        public static class UserManagement
        {
            public const string AllowSelfRegistration = "App.UserManagement.AllowSelfRegistration";
            public const string IsNewRegisteredUserActiveByDefault = "App.UserManagement.IsNewRegisteredUserActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.UserManagement.UseCaptchaOnRegistration";
            public const string IsPhoneNumberConfirmationRequiredForLogin = "App.UserManagement.IsPhoneNumberConfirmationRequiredForLogin";
        }

        public static class Security
        {
            public const string PasswordComplexity = "App.Security.PasswordComplexity";
        }

        public static class SMS
        {
            public const string DefaultSender = "App.SMS.DefaultSender";
            public const string FreeSignName = "App.SMS.FreeSignName";
            public static class Ali
            {
                public const string RegionEndpoint = "App.SMS.Ali.RegionEndpoint";
                public const string SecretAccessKey = "App.SMS.Ali.SecretAccessKey";
                public const string AccessKeyId = "App.SMS.Ali.AccessKeyId";
                public const string TopicName = "App.SMS.Ali.TopicName";
                public const string TemplateCode = "App.SMS.Ali.TemplateCode";
            }
            public static class QC
            {
                public const string Url = "App.SMS.QC.Url";
                public const string Username = "App.SMS.QC.Username";
                public const string Password = "App.SMS.QC.Password";
            }
        }


        public static class OAuth
        {
            public static class WeixinOpen
            {
                public const string IsEnabled = "App.OAuth.WeixinOpen.IsEnabled";
                public const string AppID = "App.OAuth.WeixinOpen.AppID";
                public const string AppSecret = "App.OAuth.WeixinOpen.AppSecret";
            }
            public static class Alipay
            {
                public const string IsEnabled = "App.OAuth.Alipay.IsEnabled";
                public const string AppID = "App.OAuth.Alipay.AppID";
                public const string AppPrivateKey = "App.OAuth.Alipay.AppPrivateKey";
                public const string AlipayPublicKey = "App.OAuth.Alipay.AlipayPublicKey";
            }
            public static class QQ
            {
                public const string IsEnabled = "App.OAuth.QQ.IsEnabled";
                public const string AppID = "App.OAuth.QQ.AppID";
                public const string AppKey = "App.OAuth.QQ.AppKey";
            }
            public static class Weibo
            {
                public const string IsEnabled = "App.OAuth.Weibo.IsEnabled";
                public const string AppID = "App.OAuth.Weibo.AppID";
                public const string AppSecret = "App.OAuth.Weibo.AppSecret";
            }
        }

        public static class Captcha
        {
            public static class Geetest
            {
                public const string PublicKey = "App.Captcha.Geetest.PublicKey";
                public const string PrivateKey = "App.Captcha.Geetest.PrivateKey";
            }
        }
        /// <summary>
        /// 推送
        /// </summary>
        public static class Push
        {
            /// <summary>
            /// 极光
            /// </summary>
            public static class JPush
            {
                public const string JPushAppKey = "jpush_app_key";
                public const string JPushMasterSecret = "jpush_master_secret";
            }
            /// <summary>
            /// 个推
            /// </summary>
            public static class Getui
            {
                public const string AppID = "AppID";
                public const string AppKey = "AppKey";
                public const string MasterSecret = "MasterSecret";
            }
        }
    }
}
