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
    public class IsUpatedInput: GetAppStartPageInput
    {

        /// <summary>
        /// 本地图片的更新时间
        /// </summary>
        public virtual DateTime? UpdateTime { get; set; }
    }
}
