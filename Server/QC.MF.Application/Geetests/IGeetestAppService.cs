using Abp.Application.Services;
using Abp.Web.Models;
using QC.MF.DragVerifications.Dto;
using QC.MF.Geetests.Dto;

namespace QC.MF.Geetests
{
    public interface IGeetestAppService : IApplicationService
    {
        string GetCaptcha();
        CheckCodeOutput Check(GeetestCheckInput input);
        [DontWrapResult]
        GeetestCheckOutput APPGetCaptcha();
        [DontWrapResult]
        string APPCheck(GeetestAppCheckInput input);
    }
}
