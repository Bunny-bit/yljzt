using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using QC.MF.AppEditions;
using QC.MF.Geetests;

namespace QC.MF.Api
{
    [DependsOn(typeof(AbpWebApiModule), typeof(MFApplicationModule))]
    public class MFWebApiModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;
            base.PreInitialize();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(MFApplicationModule).Assembly, "app")
                .Build();
            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .For<IAppEditionsAppService>("app/appEditions")
                .ForMethod("DownloadAppEdition").WithVerb(Abp.Web.HttpVerb.Get)
                .Build();
            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .For<IGeetestAppService>("app/geetest")
                .ForMethod("APPGetCaptcha").WithVerb(Abp.Web.HttpVerb.Get)
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
        }
    }
}
