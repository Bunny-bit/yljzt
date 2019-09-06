using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Captcha
{
    public class VerifcationCache
    {
        public VerifcationType VerifcationType { get; set; }
        public int ErrorCount { get; set; }
        public string Code { get; set; }
    }
}
