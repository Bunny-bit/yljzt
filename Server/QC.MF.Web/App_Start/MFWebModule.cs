using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.WebApi.ExceptionHandling;
using Abp.Zero.Configuration;
using QC.MF.Api;
using Hangfire;

namespace QC.MF.Web
{
    [DependsOn(
        typeof(MFDataModule),
        typeof(MFApplicationModule),
        typeof(MFWebApiModule),
        typeof(AbpWebSignalRModule),
        //typeof(AbpHangfireModule), - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(AbpWebMvcModule))]
    public class MFWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            
            //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
