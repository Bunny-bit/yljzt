using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using QC.MF.Demos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Demos.Dto
{
    [AutoMap(typeof(FileSettingDemo))]
    public class SetFileSettingDemoDto
    {
        /// <summary>
        /// 文件大小限制
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件格式限制
        /// </summary>
        public string FileExtension { get; set; }
    }
}
