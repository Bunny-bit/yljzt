using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration.Dto
{
    public class SettingsEditOutput
    {
        public List<SettingProperty> Setting { get; set; }
        public string TabName { get; set; }
        /// <summary>
        /// 用于反射获取dto
        /// </summary>
        public string Name { get; set; }
    }
}
