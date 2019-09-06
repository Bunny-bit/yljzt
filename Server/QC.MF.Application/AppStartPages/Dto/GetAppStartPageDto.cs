using Abp.AutoMapper;
using QC.MF.CommonDto;
using QC.MF.PreviousAndNexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppStartPages.Dto
{
    [AutoMap(typeof(AppStartPage))]
    public class GetAppStartPageDto:GetListAppStartPageDto
    {
    }
}
