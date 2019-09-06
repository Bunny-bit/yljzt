using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Authorization
{
    [Flags]
    public enum LoginSettingVerificationResult
    {
        Ok = 0,
        NeedPhoneNumberConfirmation = 1,
        NeedEmailConfirmation = 2,
    }
}
