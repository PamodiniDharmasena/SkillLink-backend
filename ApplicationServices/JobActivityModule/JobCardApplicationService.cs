using SkillLink.BusinessObjects.JobActivityModule;
using SkillLink.BusinessObjects.ProfileModule;
using SkillLink.DataServices.JobActivityModule;
using SkillLink.DataServices.ProfileModule;

namespace SkillLink.ApplicationServices.JobActivityModule
{

   
    public class JobCardApplicationService
    {
        private readonly JobCardDataService _jobCardDataService;

        public JobCardApplicationService(JobCardDataService jobCardDataService)
        {
            _jobCardDataService = jobCardDataService;
        }
        public async Task<List<JobCard>> GetJobCardDetails(JobCardFilterObj jobCardFilterObj)
        {
            if(jobCardFilterObj.NumberOfViewsStart == null)
            {
                jobCardFilterObj.NumberOfViewsStart = 0;
            }

            string CategoryList = "";

            if (jobCardFilterObj.Category != null)
            {
                CategoryList = string.Join(",", jobCardFilterObj.Category);
            }
 


            var jobCardDetails = await _jobCardDataService.GetJobCardDetailsAsync(jobCardFilterObj, CategoryList);
            return jobCardDetails;
        }



        public async Task<List<string>> GetJobCategories()
        {
            var jobCategories = await _jobCardDataService.GetJobCategoriesAsync();
            return jobCategories;
        }
        

        public async Task<JobDetailsAsWorkerAll> GetJobDetailsAsWorker(int workerId)
        {
            var jobDetailsForWorker = await _jobCardDataService.GetJobDetailsAsWorkerAsync(workerId);
            return jobDetailsForWorker;
        }



        public async Task<List<JobHiringModeDetail>> GetJobDetailsHiringMode(int userId)
        {
            var response= await _jobCardDataService.GetJobDetailsHiringModeAsync(userId);



            foreach(var itemHiringMode in response.JobHiringModeDetails)
            {
                foreach(var itemBedding in response.JobBeddingDetails)
                {
                    if(itemHiringMode.JobId == itemBedding.JobId)
                    {
                        itemHiringMode.JobBeddingInfoList.Add(itemBedding);
                    }
                }
            }

            return response.JobHiringModeDetails;
        }

        public async Task<bool> ChangeAcceptancyForPostedJob(JobAcceptancyChangeForPostedJob jobAcceptancyChangeForPostedJob)
        {
            var result = await _jobCardDataService.ChangeAcceptancyForPostedJobAsync(jobAcceptancyChangeForPostedJob);
            return result;
        }
        

        public async Task<bool> WorkerAcceptOrRejectJob(JobAcceptancyByWorker jobAcceptancyByWorker)
        {
            var result = await _jobCardDataService.WorkerAcceptOrRejectJobAsync(jobAcceptancyByWorker);
            return result;
        }

        public async Task<bool> SaveNewJob(JobPost job)
        {
            var result = await _jobCardDataService.SaveNewJobAsync(job);
            return result;
        }


    }
}
