using Abp.Configuration;
using Abp.Json;
using Abp.Zero.Configuration;
using Newtonsoft.Json;
using QC.MF.Configuration;
using QC.MF.Configuration.Dto;
using QC.MF.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration
{
    public class SecurityConfigurationService : ConfigurationService<SecuritySettingDto>, IConfigurationService<SecuritySettingDto>
    {

        public override SettingsEditOutput GetSetting()
        {
            var result = base.GetSetting();

            var passwordComplexity = result.Setting.Find(n => n.Name == "PasswordComplexity");
            passwordComplexity.Value = SettingManager.GetSettingValueForApplication(AppSettingNames.Security.PasswordComplexity);

            if (passwordComplexity.Value.ToString() == PasswordComplexitySetting.DefaultPasswordComplexitySetting.ToJsonString())
            {
                var useDefaultPasswordComplexity = result.Setting.Find(n => n.Name == "UseDefaultPasswordComplexity");
                useDefaultPasswordComplexity.Value = true;
            }

            return result;
        }


        public override void SetSetting(SecuritySettingDto input)
        {
            base.SetSetting(input);
            SettingManager.ChangeSettingForApplication(AppSettingNames.Security.PasswordComplexity, JsonConvert.SerializeObject(input.PasswordComplexity));
        }
    }
}
