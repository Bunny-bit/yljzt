using Abp.AutoMapper;
using QC.MF.CommonDto;
using QC.MF.PreviousAndNexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppStartPages.Dto
{
    public class GetAppStartPageInput
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
    }
}
