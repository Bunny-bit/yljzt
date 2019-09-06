using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Users.Dto
{
    public class PhoneInput
    {
        [Required]
        [RegularExpressionAttribute(@"^1[34578]\d{9}$", ErrorMessage = "手机号格式错误")]
        public string PhoneNumber { get; set; }
    }
}
