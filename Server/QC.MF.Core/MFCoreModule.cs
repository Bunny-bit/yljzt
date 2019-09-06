using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using QC.MF.Authorization;
using QC.MF.Authorization.Roles;
using QC.MF.Authorization.Users;
using QC.MF.Configuration;
using QC.MF.MultiTenancy;
using Abp.Configuration;
using Castle.MicroKernel.Registration;
using QC.MF.SMSs;
using System.Linq;
using QC.MF.Notifications;
using QC.MF.Features;

namespace QC.MF
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class MFCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            //Remove the following line to disable multi-tenancy.
            Configuration.MultiTenancy.IsEnabled = MFConsts.MultiTenancyEnabled;

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    MFConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "QC.MF.Localization.Source"
                        )
                    )
                );
            Configuration.Features.Providers.Add<AppFeatureProvider>();

            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Authorization.Providers.Add<MFAuthorizationProvider>();

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.Notifications.Providers.Add<AppNotificationProvider>();

            Configuration.Localization.WrapGivenTextIfNotFound = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            var settingManager = IocManager.Resolve<ISettingManager>();
            var defaultSender = settingManager.GetSettingValue(AppSettingNames.SMS.DefaultSender);
            var senders = IocManager.IocContainer.ResolveAll<ISMSSenderManager>();
            var senderType = senders.FirstOrDefault(n => n.GetType().Name.Contains(defaultSender))?.GetType();
            if (senderType != null)
            {
                IocManager.IocContainer.Register(
                    Component.For<ISMSSenderManager>().Named(defaultSender).ImplementedBy(senderType).IsDefault()
                );
            }

            IocManager.IocContainer.Release(senders);
            IocManager.IocContainer.Release(settingManager);
            base.PostInitialize();
        }
    }
}
