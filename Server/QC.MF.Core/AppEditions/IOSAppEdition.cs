using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppEditions
{
    public class IOSAppEdition : AppEdition
    {
        /// <summary>
        /// Itunes连接
        /// </summary>
        [StringLength(500)]
        public string ItunesUrl { get; set; }
    }
}
