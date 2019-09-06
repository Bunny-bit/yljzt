using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Users.Dto
{
    public class VerificationCodeInput : PhoneInput
    {
        public string Code { get; set; }
    }
}
