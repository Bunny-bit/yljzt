using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace QC.MF.Demos
{
    /// <summary>
    /// 设置
    /// </summary>
    public class FileSettingDemo : FullAuditedEntity
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
