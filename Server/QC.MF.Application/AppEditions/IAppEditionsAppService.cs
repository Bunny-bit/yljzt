using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using QC.MF.AppEditions.Dto;
using Abp.Application.Services;
using System;

namespace QC.MF.AppEditions
{
    public interface IAppEditionsAppService : IApplicationService
    {
        Task CreateAndroidAppEdition(CreateAndroidAppEditionInput input);
        Task CreateIOSAppEdition(CreateIOSAppEditionInput input);
        Task DeleteAppEdition(EntityDto input);
        Task<PagedResultDto<AppEditionDto>> GetAppEditions(GetAppEditionsInput input);
        Task UpdateAndroidAppEdition(UpdateAndroidAppEditionInput input);
        Task UpdateIOSAppEdition(UpdateIOSAppEditionInput input);
        Task<Guid> UploadAppEdition();
        Task DownloadAppEdition(int id);
        Task<AboutOutput> GetAbout(VersionInput input);
        Task<CheckUpdateOutput> CheckUpdate(VersionInput input);
        Task<GetAboutAndCheckUpdateOutput> GetAboutAndCheckUpdate(VersionInput input);
    }
}
