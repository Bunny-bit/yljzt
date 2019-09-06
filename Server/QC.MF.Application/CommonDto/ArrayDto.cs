using System.ComponentModel.DataAnnotations;

namespace QC.MF.CommonDto
{
    /// <summary>
    /// 数组信息
    /// </summary>
    public class ArrayDto<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        [Required]
        public T[] Value { get; set; }
    }
}
