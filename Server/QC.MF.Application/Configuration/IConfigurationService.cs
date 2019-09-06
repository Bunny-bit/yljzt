using System.Collections.Generic;
using QC.MF.Configuration.Dto;
using Abp.Application.Services;
using Abp.Dependency;

namespace QC.MF.Configuration
{
    public interface IConfigurationService<T> where T : ISettingDto
    {
        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        SettingsEditOutput GetSetting();
        /// <summary>
        /// 应用设置
        /// </summary>
        /// <param name="input"></param>
        void SetSetting(T input);
    }
}
