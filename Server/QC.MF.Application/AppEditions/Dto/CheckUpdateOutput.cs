using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppEditions.Dto
{
    public class CheckUpdateOutput : AboutOutput
    {
        public string Version { get; set; }
        public string DownloadtUrl { get; set; }
        public string ItunesUrl { get; set; }
        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool IsMandatoryUpdate { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}
