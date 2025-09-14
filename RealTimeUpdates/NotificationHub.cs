using Microsoft.AspNetCore.SignalR;
using System.Data.SqlClient;

namespace SkillLink.RealTimeUpdates
{
    public class NotificationHub : Hub
    {
        private readonly string _connectionString;

        public NotificationHub(IConfiguration configuration)
        {
            // Get connection string from appsettings.json (default connection)
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Start SQL Dependency Listener
        public void StartSqlDependency()
        {
            SqlDependency.Start(_connectionString);
            GetNotification();
        }

        private void GetNotification()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID FROM dbo.Notification", connection))
                {
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(OnNotificationChange);
                    command.ExecuteReader();
                }
            }
        }

        // When a change occurs in the Notification table, send "true"
        private void OnNotificationChange(object sender, SqlNotificationEventArgs e)
        {
            // Log the change event
            Console.WriteLine($"Notification change detected. Type: {e.Type}, Info: {e.Info}, Source: {e.Source}");

            if (e.Type == SqlNotificationType.Change)
            {
                // Notify the relevant user by sending "true"
                SendNotificationToClients(true);
            }

            // Re-establish the notification listener
            GetNotification();
        }

        // Send true to all clients or to specific users based on your logic
        private void SendNotificationToClients(bool isChanged)
        {
            // Log the notification sending
            Console.WriteLine("Sending notification to clients");

            // Broadcast to all connected users (you can change this to target specific users)
            Clients.All.SendAsync("ReceiveNotification", isChanged);
        }
    }
}
