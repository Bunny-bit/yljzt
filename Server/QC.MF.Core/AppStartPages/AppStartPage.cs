using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppStartPages
{
    public  class AppStartPage: FullAuditedEntity
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

        public bool IsUpdated(DateTime? date)
        {
            if (!date.HasValue)
            {
                return true;
            }
            var updateTime = LastModificationTime ?? CreationTime;
            return (updateTime - date.Value).TotalSeconds >= 2;
        }
    }
}
