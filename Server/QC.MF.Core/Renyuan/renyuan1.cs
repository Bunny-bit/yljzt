using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Renyuan
{
    public class Renyuan1:Entity<int>
    {
        public string xingming { get;set;}
        public string banji { get; set; }
        public int xuehao { get; set; }
        public string xueyuan { get; set; }
    }
}
