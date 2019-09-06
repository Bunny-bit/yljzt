using System.Text.RegularExpressions;
using Abp.Extensions;
using Newtonsoft.Json;
using Abp.Configuration;
using QC.MF.Configuration;
using Abp.UI;

namespace QC.MF.Security
{
    public class PasswordComplexityChecker:MFDomainServiceBase
    {
        public void Check( string password)
        {
            var passwordComplexitySettingValue =  SettingManager.GetSettingValue(AppSettingNames.Security.PasswordComplexity);
            var passwordSetting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(passwordComplexitySettingValue);

            var errorMsg = "";
            if (passwordSetting == null)
            {
                return;
            }
            if (password.Length < passwordSetting.MinLength)
            {
                errorMsg += $"密码长度不能少于{passwordSetting.MinLength}位。  ";
            }
            if (password.Length > passwordSetting.MaxLength)
            {
                errorMsg += $"密码长度不能超过{passwordSetting.MaxLength}位。  ";
            }
            if (passwordSetting.UseNumbers)
            {
                if (!Regex.Match(password, "\\d").Success)
                {
                    errorMsg += "密码必须包含数字。  ";
                }
            }
            if (passwordSetting.UseUpperCaseLetters)
            {
                if (!Regex.Match(password, "[A-Z]").Success)
                {
                    errorMsg += "密码必须包含大写字母 。  ";
                }
            }
            if (passwordSetting.UseLowerCaseLetters)
            {
                if (!Regex.Match(password, "[a-z]").Success)
                {
                    errorMsg += "密码必须包含小写字母。  ";
                }
            }
            if (passwordSetting.UsePunctuations)
            {
                if (!Regex.Match(password, "\\p{P}").Success)
                {
                    errorMsg += "密码必须包含特殊字符（如!@#$） 。  ";
                }
            }

            if (errorMsg.Length > 0)
            {
                throw new UserFriendlyException(errorMsg);
            }
        }
    }
}
