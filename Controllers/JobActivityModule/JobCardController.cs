using Microsoft.AspNetCore.Mvc;
using SkillLink.ApplicationServices.JobActivityModule;
using SkillLink.BusinessObjects.JobActivityModule;

namespace SkillLink.Controllers.JobActivityModule
{


    [ApiController]
    [Route("api/v1.0/[controller]/[action]")]
    public class JobCardController : ControllerBase
    {
        private readonly JobCardApplicationService _jobCardApplicationService;

        public JobCardController(JobCardApplicationService jobCardApplicationService)
        {
            _jobCardApplicationService = jobCardApplicationService;
        }

        [HttpPost(Name = "GetJobCardDetails")]
        public async Task<IActionResult> GetJobCardDetails([FromBody] JobCardFilterObj jobCardFilterObj)
        {
            try
            {
                var jobCardDetails = await _jobCardApplicationService.GetJobCardDetails(jobCardFilterObj);
                return Ok(jobCardDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet(Name = "GetJobCategories")]
        public async Task<IActionResult> GetJobCategories()
        {
            try
            {
                var jobCategories = await _jobCardApplicationService.GetJobCategories();
                return Ok(jobCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet(Name = "GetJobDetailsAsWorker")]
        public async Task<IActionResult> GetJobDetailsAsWorker(int workerId)
         {
            try
            {
                var jobDetailsForWoker = await _jobCardApplicationService.GetJobDetailsAsWorker(workerId);
                return Ok(jobDetailsForWoker);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet(Name = "GetJobDetailsHiringMode")]
        public async Task<IActionResult> GetJobDetailsHiringMode(int userId)
        {
            try
            {
                var jobDetailHiringMode = await _jobCardApplicationService.GetJobDetailsHiringMode(userId);
                return Ok(jobDetailHiringMode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(Name = "ChangeAcceptancyForPostedJob")]
        public async Task<IActionResult> ChangeAcceptancyForPostedJob(JobAcceptancyChangeForPostedJob jobAcceptancyChangeForPostedJob)
        {
            try
            {
                var result = await _jobCardApplicationService.ChangeAcceptancyForPostedJob(jobAcceptancyChangeForPostedJob);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost(Name = "WorkerAcceptOrRejectJob")]
        public async Task<IActionResult> WorkerAcceptOrRejectJob(JobAcceptancyByWorker jobAcceptancyByWorker)
        {
            try
            {
                var result = await _jobCardApplicationService.WorkerAcceptOrRejectJob(jobAcceptancyByWorker);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "SaveNewJob")]
        public async Task<IActionResult> SaveNewJob(JobPost job)
        {
            try
            {
                var result = await _jobCardApplicationService.SaveNewJob(job);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
