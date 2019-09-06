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
                new MessageNotificationData("��ӭʹ�ñ�ϵͳ"),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() }
                );
        }

        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new MessageNotificationData("��һ�����û�ע����");

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;
            notificationData["phoneNumber"] = user.PhoneNumber;
            notificationData["content"] = $"�û�������{user.UserName}�����ֻ��ţ���{user.PhoneNumber}�������䣺��{user.EmailAddress}����";


            await _notificationPublisher.PublishAsync(AppNotificationNames.NewUserRegistered, notificationData, tenantIds: new[] { user.TenantId });
        }

        public async Task NewTenantRegisteredAsync(Tenant tenant)
        {
            var notificationData = new MessageNotificationData("��һ�����⻧ע����");

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
