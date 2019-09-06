using Abp.Zero.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration.Dto
{
    [Tab("用户管理设置", 1)]
    public class UserManagementSettingDto : ISettingDto
    {
        [Title("身份验证设置")]
        [DisplayName("允许用户注册")]
        [Description("如果此项被禁用，只能由管理员通过用户管理页面添加用户")]
        [Key(AppSettingNames.UserManagement.AllowSelfRegistration)]
        public bool AllowSelfRegistration { get; set; }
        //[DisplayName("注册用户默认激活")]
        //[Description("如果此项被禁用，新用户需要通过短信激活后才能登录")]
        //[Value(AppSettingNames.UserManagement.IsNewRegisterQC.MFserActiveByDefault)]
        //public bool IsNewRegisterQC.MFserActiveByDefault { get; set; }
        //[DisplayName("用户注册时使用图片验证码")]
        //[Value(AppSettingNames.UserManagement.UseCaptchaOnRegistration)]
        //public bool UseCaptchaOnRegistration { get; set; }

        [Title("其他设置")]
        [DisplayName("必须验证邮箱地址后才能登录")]
        [Key(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin)]
        public bool IsEmailConfirmationRequiredForLogin { get; set; }
        [DisplayName("必须验证手机后才能登录")]
        [Key(AppSettingNames.UserManagement.IsPhoneNumberConfirmationRequiredForLogin)]
        public bool IsPhoneNumberConfirmationRequiredForLogin { get; set; }
    }
}
