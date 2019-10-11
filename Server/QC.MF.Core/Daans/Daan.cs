using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Daans
{
    public class Daan : Entity
    {
        public int RenyuanId{get;set;}

        public  int TimuId { get; set; }

        public int XuanxiangId { get; set; }

    }
}
