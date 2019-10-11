using Abp.AutoMapper;
using QC.MF.Demos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Renyuan.Dto
{
    [AutoMap(typeof(Renyuan1))]
    public class CreateRenyuanDto
    {
        public string xingming { get; set; }
        public string banji { get; set; }
        public int xuehao { get; set; }
        public string xueyuan { get; set; }
    }
}
