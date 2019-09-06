using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppEditions.Dto
{
    [AutoMap(typeof(IOSAppEdition))]
    public class CreateIOSAppEditionInput : CreateAppEditionInput
    {
        /// <summary>
        /// Itunes连接
        /// </summary>
        [StringLength(500)]
        [Url]
        public string ItunesUrl { get; set; }
    }
}
