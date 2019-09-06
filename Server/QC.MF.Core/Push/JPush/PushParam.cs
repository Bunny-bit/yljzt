using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.JPush
{
    /// <summary>
    /// 推送参数
    /// </summary>
    public class PushParam
    {
        /// <summary>
        /// 参数
        /// </summary>
        public string Param { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 来源Id
        /// </summary>
        public string SourceId { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("Id", ((int)Type).ToString());
            //dic.Add("Type", Enum.GetName(typeof(ContentType), Type));
            dic.Add("Param", Param);
            dic.Add("Source", Source);
            dic.Add("SourceId", SourceId);
            return dic;
        }
    }
}
