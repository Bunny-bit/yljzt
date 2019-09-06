using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.DragVerifications.Dto
{
    public class DragVerificationDto
    {
        /// <summary>
        /// 裁剪图片y轴位置
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 无序数组
        /// </summary>
        public int[] Array { get; set; }
        /// <summary>
        /// 原图宽
        /// </summary>
        public int ImgX { get; set; }
        /// <summary>
        /// 原图高
        /// </summary>
        public int ImgY { get; set; }
        /// <summary>
        /// 小图字符串
        /// </summary>
        public string Small { get; set; }
        /// <summary>
        /// 原图高
        /// </summary>
        public string Normal { get; set; }
    }
}
