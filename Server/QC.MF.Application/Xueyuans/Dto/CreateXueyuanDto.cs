using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Xueyuans.Dto
{
    [AutoMap(typeof(Xueyuan.Xueyuan))]
    public class  CreatexueyuanDto
    {
        public string Name { get; set; }
    }
}
