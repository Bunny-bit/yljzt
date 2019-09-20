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
        public  string DaanId { get; set; }
        public string DaanNeirong { get; set; }
    }
}
