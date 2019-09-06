using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Geetests.Dto
{
    public class GeetestAppCheckInput
    {
        [Required]
        public string geetest_challenge { get; set; }
        [Required]
        public string geetest_validate { get; set; }
        [Required]
        public string geetest_seccode { get; set; }
    }
}
