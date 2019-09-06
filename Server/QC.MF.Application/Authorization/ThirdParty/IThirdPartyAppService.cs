using Abp.Application.Services;

namespace QC.MF.Authorization.ThridParty
{
    public interface IThirdPartyAppService:IApplicationService
    {
        string GetRedirectUrlList();
    }
}
