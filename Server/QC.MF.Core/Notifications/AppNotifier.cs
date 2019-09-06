using System.Threading.Tasks;
using Abp;
using Abp.Localization;
using Abp.Notifications;
using QC.MF.Authorization.Users;
using QC.MF.MultiTenancy;
using Abp.Domain.Services;

namespace QC.MF.Notifications
{
    public class AppNotifier : MFDomainServiceBase, IAppNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;

        public AppNotifier(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task WelcomeToTheApplicationAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData("欢迎使用本系统"),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() }
                );
        }

        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new MessageNotificationData("有一个新用户注册了");

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;
            notificationData["phoneNumber"] = user.PhoneNumber;
            notificationData["content"] = $"用户名：【{user.UserName}】，手机号：【{user.PhoneNumber}】，邮箱：【{user.EmailAddress}】。";


            await _notificationPublisher.PublishAsync(AppNotificationNames.NewUserRegistered, notificationData, tenantIds: new[] { user.TenantId });
        }

        public async Task NewTenantRegisteredAsync(Tenant tenant)
        {
            var notificationData = new MessageNotificationData("有一个新租户注册了");

            notificationData["tenancyName"] = tenant.TenancyName;
            await _notificationPublisher.PublishAsync(AppNotificationNames.NewTenantRegistered, notificationData);
        }

        //This is for test purposes
        public async Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] { user }
                );
        }
    }
}
