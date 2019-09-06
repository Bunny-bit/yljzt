using Abp.Runtime.Validation;
using Abp.Zero.Configuration;
using QC.MF.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration.Dto
{
    [Tab("安全设置", 2)]
    public class SecuritySettingDto : IShouldNormalize, ISettingDto
    {
        [Title("密码复杂性")]
        [DisplayName("密码组成")]
        public PasswordComplexitySetting PasswordComplexity { get; set; }
        [DisplayName("使用默认配置")]
        public bool UseDefaultPasswordComplexity { get; set; }

        // fany: 此配置项使用微软Aspnet.Identity，此配置项的作用是标识新建用户是否启用账号锁定，
        //       和我们需求不一致，去除此配置
        //[DisplayName("登录失败后启用用户的帐户锁定")]
        //[Value(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled)]
        //[Title("用户锁定")]
        //public bool IsEnabled { get; set; }

        [Title("用户锁定")]
        [DisplayName("在锁定帐户之前的累计登录失败的最大数量")]
        [Key(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout)]
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }
        [DisplayName("帐户锁定持续时间（秒）")]
        [Key(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds)]
        public int DefaultAccountLockoutSeconds { get; set; }



        public void Normalize()
        {
            if (UseDefaultPasswordComplexity == true)
            {
                PasswordComplexity = PasswordComplexitySetting.DefaultPasswordComplexitySetting;
            }
        }
    }
}
