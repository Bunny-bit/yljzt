using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppStartPages.Dto
{
    [AutoMap(typeof(AppStartPage))]
    public class CreateAppStartPageDto
    {
        /// <summary>
        /// 平台 Android = 1, IOS = 2 
        /// （创建默认值时，该项可空）
        /// </summary>
        public Platform? Platform { get; set; }

        /// <summary>
        /// 分辨率 宽
        /// （创建默认值时，该项可空）
        /// </summary>
        public int? Width_Px { get; set; }

        /// <summary>
        /// 分辨率 高
        /// （创建默认值时，该项可空）
        /// </summary>
        public int? High_Px { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string Url { get; set; }
    }
}
