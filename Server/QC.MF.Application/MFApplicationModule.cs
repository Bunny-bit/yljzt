using System.Reflection;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Modules;
using QC.MF.Authorization.Roles;
using QC.MF.Authorization.Users;
using QC.MF.Roles.Dto;
using QC.MF.Users.Dto;
using System;
using QC.MF.Authorization;
using Abp.IO;
using System.Web;
using QC.MF.Menus;
using QC.MF.Configuration;
using Castle.MicroKernel.Registration;
using QC.MF.Configuration.Dto;

namespace QC.MF
{
    [DependsOn(typeof(MFCoreModule), typeof(AbpAutoMapperModule))]
    public class MFApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {

            //Adding custom AutoMapper mappings
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                CustomDtoMapper.CreateMappings(mapper);
            });

            Configuration.Navigation.Providers.Add<DBNavigationProvider>();
            Configuration.Navigation.Providers.Add<CodeNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());


            IocManager.IocContainer.Register(
                Component.For(typeof(IConfigurationService<>))
                .ImplementedBy(typeof(ConfigurationService<>)),

                Component.For(typeof(IConfigurationService<SecuritySettingDto>))
                .Named("SecurityConfigurationService")
                .ImplementedBy(typeof(SecurityConfigurationService))
            );

        }


        public override void PostInitialize()
        {
            var server = HttpContext.Current.Server;
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = server.MapPath("~/Common/Images/SampleProfilePics");
            appFolders.ImagesFolder = server.MapPath("~/Common/Images/UserPics");
            appFolders.TempFileDownloadFolder = server.MapPath("~/Temp/Downloads");
            appFolders.WebLogsFolder = server.MapPath("~/App_Data/Logs");
            appFolders.DragVerificationImageFolder = server.MapPath("~/App_Data/DragVerificationImage");

            try { DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder); } catch { }
        }
    }
}
