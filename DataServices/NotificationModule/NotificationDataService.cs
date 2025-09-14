


using SkillLink.BusinessObjects.JobActivityModule;
using SkillLink.BusinessObjects.NotificationModule;
using System.Data.SqlClient;

namespace SkillLink.DataServices.NotificationModule
{


    public class NotificationDataService
    {
        private readonly DatabaseService _databaseService;

        public NotificationDataService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<Notification>> GetNotificationAsync(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                   new SqlParameter("@UserId", userId), 
                };

                var notificationList = new List<Notification>();

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[realTimeUpdate].[GetNotification]", parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        var notification = new Notification
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            NotificationMessage = reader.GetString(reader.GetOrdinal("NotificationMessage")),

                        };

                        notificationList.Add(notification);
                    }
                }

                return notificationList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving notifications", ex);
            }
        }


    }
}
