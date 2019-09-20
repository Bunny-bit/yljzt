using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;


namespace QC.MF.Xuanxiangs.Dto
{
    [AutoMap(typeof(Xuanxiang))]
    public  class CreateXuanxiangDto
    {
        public int TimuId { get; set; }
        public string Name { get; set; }
        public string Neirong { get; set; }

    }
}
