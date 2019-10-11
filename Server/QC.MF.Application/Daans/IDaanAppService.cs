using Abp.Application.Services;
using QC.MF.Daans.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Daans
{
    public interface IDaanAppService: IApplicationService
    {
        Task<AnswerOutput> Answer(AnswerInput input);
    }
}
