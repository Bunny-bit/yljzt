using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Xuanxiangs
{
   public class Xuanxiang : Entity
    {
        public int TimuId { get; set; }
        public string Name { get; set; }
        public string Neirong { get; set; }
        public bool IsRight { get; set; }
    }
}
