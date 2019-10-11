using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Renyua
{
    public class Renyua1:Entity<int>
    {
        public string Name { get; set; }


        public string Xueyua { get; set; }


        public string Xuehao { get; set; }


        public string Banji { get; set; }

        public string Code { get; set; }
    }
}
