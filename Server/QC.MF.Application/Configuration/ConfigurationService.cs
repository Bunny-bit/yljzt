using QC.MF.Configuration.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Abp.Configuration;
using Abp.Authorization;
using QC.MF.Authorization;

namespace QC.MF.Configuration
{
    public class ConfigurationService<T> : MFAppServiceBase, IConfigurationService<T> where T : ISettingDto
    {
        [AbpAuthorize(PermissionNames.Pages_Administration_Settings)]
        public virtual SettingsEditOutput GetSetting()
        {
            var list = typeof(T).GetProperties().Select(n => new SettingProperty()
            {
                Name = n.Name,
                DisplayName = n.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault()?.DisplayName,
                Description = n.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description,
                Type = n.PropertyType.Name.ToLower(),
                Value = n.GetCustomAttributes<KeyAttribute>().FirstOrDefault() == null ? null : Convert.ChangeType(SettingManager.GetSettingValue(n.GetCustomAttributes<KeyAttribute>().FirstOrDefault()?.Value), n.PropertyType),
                Title = n.GetCustomAttributes<TitleAttribute>().FirstOrDefault()?.Title,
            }).ToList();

            return new SettingsEditOutput()
            {
                Setting = list,
                Name = typeof(T).Name,
                TabName = typeof(T).GetCustomAttributes<TabAttribute>().FirstOrDefault()?.TabName,
            };
        }
        [AbpAuthorize(PermissionNames.Pages_Administration_Settings)]
        public virtual void SetSetting(T input)
        {
            typeof(T).GetProperties().ToList().ForEach(n =>
            {
                var name = n.GetCustomAttributes<KeyAttribute>().FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(name))
                {
                    var value = n.GetValue(input).ToString();
                    if (n.PropertyType.Name == typeof(bool).Name)
                    {
                        value = value.ToLower();
                    }
                    SettingManager.ChangeSettingForApplication(name, value);
                }
            });
        }
    }
}
