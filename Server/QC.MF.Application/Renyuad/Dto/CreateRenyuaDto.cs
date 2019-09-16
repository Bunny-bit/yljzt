using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Renyua.Dto
{
    [AutoMap(typeof(Renyua.Renyua1))]
    public class CreateRenyuaDto
    {
        public string Name { get; set; }


        public string Xueyua { get; set; }


        public int Xuehao { get; set; }


        public string Banji { get; set; }
    }
}
