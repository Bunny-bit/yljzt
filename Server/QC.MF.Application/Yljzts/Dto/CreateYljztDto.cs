using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Yljzts.Dto
{
    [AutoMap(typeof(Tiku))]
    public class CreateYljztDto
    {
        public string TiMu { get; set; }

        public int TiHao { get; set; }

    }
}
