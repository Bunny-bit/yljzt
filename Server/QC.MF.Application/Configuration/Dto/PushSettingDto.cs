using Abp.Zero.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration.Dto
{
    public class PushSettingDto
    {
        [Title("极光推送设置")]
        [DisplayName("AppKey")]
        [Key(AppSettingNames.Push.JPush.JPushAppKey)]
        public string JPushAppKey { get; set; }

        [DisplayName("MasterSecret")]
        [Key(AppSettingNames.Push.JPush.JPushMasterSecret)]
        public string JPushMasterSecret { get; set; }

        [Title("个推设置")]
        [DisplayName("AppID")]
        [Key(AppSettingNames.Push.Getui.AppID)]
        public string GetuiAppID { get; set; }

        [DisplayName("AppKey")]
        [Key(AppSettingNames.Push.Getui.AppKey)]
        public string GetuiAppKey { get; set; }

        [DisplayName("MasterSecret")]
        [Key(AppSettingNames.Push.Getui.MasterSecret)]
        public string GetuiMasterSecret { get; set; }
    }
}
