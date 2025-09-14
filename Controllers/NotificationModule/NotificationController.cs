using Microsoft.AspNetCore.Mvc;
using SkillLink.ApplicationServices.JobActivityModule;
using SkillLink.ApplicationServices.NotificationModule;
using SkillLink.BusinessObjects.JobActivityModule;
using System.Data.SqlClient;

namespace SkillLink.Controllers.NotificationModule
{

    [ApiController]
    [Route("api/v1.0/[controller]/[action]")]
    public class NotificationController : ControllerBase
    {

        private readonly NotificationApplicationService _notificationApplicationService;

        public NotificationController(NotificationApplicationService notificationApplicationService)
        {
            _notificationApplicationService = notificationApplicationService;
        }

        [HttpGet(Name = "GetNotification")]
        public async Task<IActionResult> GetNotification(int userId)
        {
            try
            {
                var notificatioList = await _notificationApplicationService.GetNotification(userId);
                return Ok(notificatioList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
