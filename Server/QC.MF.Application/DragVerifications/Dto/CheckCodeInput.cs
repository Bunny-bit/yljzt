using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.DragVerifications.Dto
{
    public class CheckCodeInput
    {
        /// <summary>
        /// 完成时x轴对于左上角位置位置
        /// </summary>
        [Required]
        public int Point { get; set; }
        /// <summary>
        /// 滑动过程特征
        /// </summary>
        public string DateList { get; set; }
        /// <summary>
        /// 耗时
        /// </summary>
        public string Timespan { get; set; }
    }
}
