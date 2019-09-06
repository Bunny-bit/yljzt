using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Geetests.Dto
{
    public class GeetestCheckOutput
    {
        public int success { get; set; }
        public string challenge { get; set; }
        public string gt { get; set; }
        public bool new_captcha { get; set; }
    }
}
