using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.JPush
{
    /// <summary>
    /// 推送类型
    /// </summary>
    public enum PushType
    {
        /// <summary>
        /// 标签推送，如角色
        /// </summary>
        Tag,
        /// <summary>
        /// 别名推送，登录名
        /// </summary>
        Alias,
        /// <summary>
        /// 所有人
        /// </summary>
        All
    }
}
