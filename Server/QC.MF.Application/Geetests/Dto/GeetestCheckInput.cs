using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Geetests.Dto
{
    public class GeetestCheckInput
    {
        [Required]
        public string Challenge { get; set; }
        [Required]
        public string Validate { get; set; }
        [Required]
        public string Seccode { get; set; }
    }
}
