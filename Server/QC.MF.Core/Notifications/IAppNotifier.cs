using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using QC.MF.Authorization.Users;
using QC.MF.MultiTenancy;

namespace QC.MF.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);
    }
}
