using SkillLink.BusinessObjects.NotificationModule;
using SkillLink.DataServices.JobActivityModule;
using SkillLink.DataServices.NotificationModule;

namespace SkillLink.ApplicationServices.NotificationModule
{
    public class NotificationApplicationService
    {
        private readonly NotificationDataService _notificationDataService;

        public NotificationApplicationService(NotificationDataService notificationDataService)
        {
            _notificationDataService = notificationDataService;
        }

        public async Task<List<Notification>> GetNotification(int id)
        {
            var notificatioList = await _notificationDataService.GetNotificationAsync(id);
            return notificatioList;
        }
    }
}
