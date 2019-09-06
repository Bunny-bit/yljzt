using Abp.Notifications;
using QC.MF.CommonDto;

namespace QC.MF.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}
