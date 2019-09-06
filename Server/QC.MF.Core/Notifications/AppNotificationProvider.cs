using Abp.Authorization;
using Abp.Localization;
using Abp.Notifications;
using QC.MF.Authorization;

namespace QC.MF.Notifications
{
    public class AppNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewUserRegistered,
                    displayName: L("新用户注册"),
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Users)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MFConsts.LocalizationSourceName);
        }
    }
}
