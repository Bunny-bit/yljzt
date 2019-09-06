using Abp.Application.Services;
using QC.MF.DragVerifications.Dto;

namespace QC.MF.DragVerifications
{
    public interface IDragVerificationAppService : IApplicationService
    {
        CheckCodeOutput CheckCode(CheckCodeInput input);
        DragVerificationDto GetDragVerificationCode();
    }
}
