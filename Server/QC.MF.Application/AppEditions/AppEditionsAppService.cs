using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using QC.MF.AppEditions.Dto;
using QC.MF.WebFiles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using QC.MF.Authorization;
using System.Web;

namespace QC.MF.AppEditions
{
    public class AppEditionsAppService : MFAppServiceBase, IAppEditionsAppService
    {
        private readonly IRepository<AppEdition> _appEditionRepository;
        private readonly IWebFileManager _webFileManager;
        public AppEditionsAppService(IRepository<AppEdition> appEditionRepository,
            IWebFileManager webFileManager)
        {
            _appEditionRepository = appEditionRepository;
            _webFileManager = webFileManager;
        }
        [AbpAuthorize(PermissionNames.Pages_Administration_AppEdition)]
        public async Task<PagedResultDto<AppEditionDto>> GetAppEditions(GetAppEditionsInput input)
        {
            var query = _appEditionRepository.GetAll();
            if (!string.IsNullOrEmpty(input.Filter))
            {
                query = query.Where(n => n.Version.Contains(input.Filter));
            }
            switch (input.AppSearchType)
            {
                case AppSearchType.IOS:
                    query = query.Where(n => n is IOSAppEdition);
                    break;
                case AppSearchType.Android:
                    query = query.Where(n => n is AndroidAppEdition);
                    break;
                default:
                    break;
            }

            var resultCount = await query.CountAsync();
            if (input.Sorting == "appType asc")
            {
                query = query.OrderBy(n => n is IOSAppEdition);
            }
            else if (input.Sorting == "appType desc")
            {
                query = query.OrderByDescending(n => n is IOSAppEdition);
            }
            else
            {
                query = query.OrderBy(input.Sorting);
            }
            var results = await query
                .PageBy(input)
                .ToListAsync();
            var list = results.MapTo<List<AppEditionDto>>();
            return new PagedResultDto<AppEditionDto>(resultCount, list);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_AppEdition)]
        public async Task CreateIOSAppEdition(CreateIOSAppEditionInput input)
        {
            if (_appEditionRepository.GetAll().Any(n => n is IOSAppEdition && n.Version == input.Version))
            {
                throw new Abp.UI.UserFriendlyException("已存在该版本号");
            }
            var iosAppEdition = input.MapTo<IOSAppEdition>();
            if (input.InstallationPackage != null)
            {
                await _webFileManager.UserFileAsync(input.InstallationPackage.Value);
            }
            await _appEditionRepository.InsertAsync(iosAppEdition);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_AppEdition)]
        public async Task CreateAndroidAppEdition(CreateAndroidAppEditionInput input)
        {
            if (_appEditionRepository.GetAll().Any(n => n is AndroidAppEdition && n.Version == input.Version))
            {
                throw new Abp.UI.UserFriendlyException("已存在该版本号");
            }
            var androidAppEdition = input.MapTo<AndroidAppEdition>();
            if (input.InstallationPackage != null)
            {
                await _webFileManager.UserFileAsync(input.InstallationPackage.Value);
            }
            await _appEditionRepository.InsertAsync(androidAppEdition);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_AppEdition)]
        public async Task UpdateIOSAppEdition(UpdateIOSAppEditionInput input)
        {
            var iosAppEdition = await _appEditionRepository.GetAsync(input.Id);
            var oldInstallationPackageId = iosAppEdition.InstallationPackage;
            input.MapTo(iosAppEdition);
            if (input.InstallationPackage != null)
            {
                await _webFileManager.UserFileAsync(input.InstallationPackage.Value, oldInstallationPackageId);
            }
            await _appEditionRepository.UpdateAsync(iosAppEdition);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_AppEdition)]
        public async Task UpdateAndroidAppEdition(UpdateAndroidAppEditionInput input)
        {
            var androidAppEdition = await _appEditionRepository.GetAsync(input.Id);
            var oldInstallationPackageId = androidAppEdition.InstallationPackage;
            input.MapTo(androidAppEdition);
            if (input.InstallationPackage != null)
            {
                await _webFileManager.UserFileAsync(input.InstallationPackage.Value, oldInstallationPackageId);
            }
            await _appEditionRepository.UpdateAsync(androidAppEdition);
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_AppEdition)]
        public async Task DeleteAppEdition(EntityDto input)
        {
            var appEdition = await _appEditionRepository.GetAsync(input.Id);
            await _appEditionRepository.DeleteAsync(appEdition);
            if (appEdition.InstallationPackage != null)
            {
                await _webFileManager.DeleteAsync(appEdition.InstallationPackage.Value);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_AppEdition)]
        public async Task<Guid> UploadAppEdition()
        {
            return await _webFileManager.UploadFileAsync();
        }

        public async Task DownloadAppEdition(int id)
        {
            var appEdition = await _appEditionRepository.GetAsync(id);
            if (appEdition.InstallationPackage != null)
            {
                await _webFileManager.DownloadFileAsync(appEdition.InstallationPackage.Value);
            }
        }
        public async Task<AboutOutput> GetAbout(VersionInput input)
        {
            AppEdition appedition = await GetAppEditionByVersion(input);
            return new AboutOutput()
            {
                AboutUrl = appedition?.AboutUrl ?? "http://www.4000871428.com/"
            };
        }

        private async Task<AppEdition> GetAppEditionByVersion(VersionInput input)
        {
            var client = HttpContext.Current.Request.Headers["client"] ?? "android";
            var query = _appEditionRepository.GetAll().Where(n => n.Version == input.Version);
            if (client == "ios")
            {
                query = query.Where(n => n is IOSAppEdition);
            }
            else
            {
                query = query.Where(n => n is AndroidAppEdition);
            }
            var appEdition = await query.FirstOrDefaultAsync();
            return appEdition;
        }

        public async Task<CheckUpdateOutput> CheckUpdate(VersionInput input)
        {
            var client = HttpContext.Current.Request.Headers["client"] ?? "android";
            var query = _appEditionRepository.GetAll().Where(n => n.IsActive); ;
            if (client == "ios")
            {
                query = query.Where(n => n is IOSAppEdition);
            }
            else
            {
                query = query.Where(n => n is AndroidAppEdition);
            }
            var appEditions = await query.ToListAsync();

            if (appEditions.Count > 0)
            {
                var newAppEdition = appEditions.OrderByDescending(n => new Version(n.Version)).First();
                if (new Version(input.Version) < new Version(newAppEdition.Version))
                {
                    return new CheckUpdateOutput()
                    {
                        AboutUrl = newAppEdition.AboutUrl,
                        Version = newAppEdition.Version,
                        DownloadtUrl = newAppEdition.InstallationPackage == null ? null : $"/api/services/app/appEditions/DownloadAppEdition?id={newAppEdition.Id}",
                        ItunesUrl = (newAppEdition as IOSAppEdition)?.ItunesUrl,
                        IsMandatoryUpdate = newAppEdition.IsMandatoryUpdate,
                        Describe = newAppEdition.Describe
                    };
                }
            }
            return null;
        }

        public async Task<GetAboutAndCheckUpdateOutput> GetAboutAndCheckUpdate(VersionInput input)
        {
            return new GetAboutAndCheckUpdateOutput()
            {
                About = await GetAbout(input),
                CheckUpdate = await CheckUpdate(input)
            };
        }
    }
}
