using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF
{
    public class MFDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected MFDomainServiceBase()
        {
            LocalizationSourceName = MFConsts.LocalizationSourceName;
        }
    }
}
