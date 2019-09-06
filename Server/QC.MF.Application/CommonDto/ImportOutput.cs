using Abp.AutoMapper;
using System;
using System.Collections.Generic;

namespace QC.MF.CommonDto
{
    /// <summary>
    /// 
    /// </summary>
    public class ImportOutput
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount { get; set; }
        /// <summary>
        /// 成功数量
        /// </summary>
        public int RepeatCount { get; set; }
        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailureCount { get; set; }
    }
}
