using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Datitus
{
    public class Datitu: Entity
    {
        public string DatiXingming { get; set; }
        public string DaanXueyuan { get; set; }
    }
}
