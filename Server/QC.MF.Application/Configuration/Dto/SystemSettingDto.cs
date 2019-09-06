using Abp.Zero.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration.Dto
{
    [Tab("基础设置", 0)]
    public class SystemSettingDto : ISettingDto
    {
        [Title("系统信息设置")]
        [DisplayName("系统标题")]
        [Key(AppSettingNames.System.SystemName)]
        public string SystemName { get; set; }
        [Title("图片上传路径设置")]
        [DisplayName("图片上传路径")]
        [Description("设置图片上传路径，基于项目的相对路径  如 ~/ Common / Images / UserPics")]
        [Key(AppSettingNames.System.ImageUploadPath)]
        public string ImageUploadPath { get; set; }
    }
}
