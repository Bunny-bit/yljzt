using Abp.AutoMapper;
using QC.MF.CommonDto;
using QC.MF.PreviousAndNexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Demos.Dto
{
    [AutoMap(typeof(Demo))]
    public class GetDemoDto:GetListDemoDto
    {
        /// <summary>
        /// 上一个 下一个
        /// </summary>
        public PreviousAndNext<Demo> PreviousAndNext { get; set; }
    }
}
