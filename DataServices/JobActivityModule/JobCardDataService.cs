using Microsoft.Extensions.Configuration.UserSecrets;
using SkillLink.BusinessObjects.JobActivityModule;
using System.Data;
using System.Data.SqlClient;

namespace SkillLink.DataServices.JobActivityModule
{
    public class JobCardDataService
    {

        private readonly DatabaseService _databaseService;

        public JobCardDataService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<JobCard>> GetJobCardDetailsAsync(JobCardFilterObj jobCardFilter, string CategoryList)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@Category", CategoryList ?? (object)DBNull.Value),
                    new SqlParameter("@Deadline", jobCardFilter.Deadline ?? (object)DBNull.Value),
                    new SqlParameter("@BudgetStart", jobCardFilter.BudgetStart ?? (object)DBNull.Value),
                    new SqlParameter("@BudgetEnd", jobCardFilter.BudgetEnd ?? (object)DBNull.Value),
                    new SqlParameter("@DifficultyStart", jobCardFilter.DifficultyStart ?? (object)DBNull.Value),
                    new SqlParameter("@DifficultyEnd", jobCardFilter.DifficultyEnd ?? (object)DBNull.Value),
                    new SqlParameter("@NumberOfViewsStart", jobCardFilter.NumberOfViewsStart ?? (object)DBNull.Value),
                    new SqlParameter("@NumberOfViewsEnd", jobCardFilter.NumberOfViewsEnd ?? (object)DBNull.Value),
                    new SqlParameter("@UserId", jobCardFilter.UserId)
                };

                var jobCardList = new List<JobCard>();

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[job].[GetJobCardDetails]", parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        var jobCard = new JobCard
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            UserId = reader.GetInt32(reader.GetOrdinal("JobOwnerId")),
                            JobTitle = reader["JobTitle"].ToString(),
                            JobCategory = reader["JobCategory"].ToString(),
                            PostedDate = reader.GetDateTime(reader.GetOrdinal("PostedDate")),
                            Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline")),
                            LevelOfDifficulty = reader.GetInt32(reader.GetOrdinal("LevelOfDifficulty")),
                            JobDescription = reader["JobDescription"].ToString(),
                            Attachments = reader["Attachments"].ToString(),
                            ClientEstimateBudget = reader.GetDecimal(reader.GetOrdinal("ClientEstimateBudget")),
                            ProfileImageLink = reader["ProfileImageLink"].ToString(),
                            LoyaltyRatingAsClient = !reader.IsDBNull(reader.GetOrdinal("LoyaltyRatingAsClient"))
                             ? reader.GetDecimal(reader.GetOrdinal("LoyaltyRatingAsClient")): 0
                        };

                        jobCardList.Add(jobCard);
                    }
                }

                return jobCardList;
            }
            catch (Exception ex)
            {
                // Log the exception as necessary
                throw new Exception("Error retrieving job card details", ex);
            }
        }



        public async Task<List<string>> GetJobCategoriesAsync()
        {
            try
            {
                var parameters = new SqlParameter[]{};
                var jobCatogories = new List<string>();

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[job].[GetCategories]", parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        string jobCategory = reader["jobCategory"].ToString();
                        jobCatogories.Add(jobCategory);
                    }
                }
                return jobCatogories;
            }
            catch (Exception ex)
            {
                // Log the exception as necessary
                throw new Exception("Error retrieving job category list", ex);
            }
        }



        public async Task<JobDetailsAsWorkerAll> GetJobDetailsAsWorkerAsync(int workerId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                   new SqlParameter("@WorkerId", workerId)
                };

                var jobDetailsForWorkerAll = new JobDetailsAsWorkerAll();
                jobDetailsForWorkerAll.TotalEarnings = 0;
                jobDetailsForWorkerAll.NumberOfJobsCompleted = 0;

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[job].[GetJobDetailsForWorkerMode]", parameters))
                {
                    // Read the first result set (Job Details)
                    while (await reader.ReadAsync())
                    {
                        var jobDetailsForWorkerObj = new JobDetailsForWorker
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            JobOwnerId = reader.GetInt32(reader.GetOrdinal("JobOwnerId")),
                            PostedDate = reader.GetDateTime(reader.GetOrdinal("postedDate")).ToString("yyyy-MM-dd HH:mm:ss"),
                            Deadline = reader.GetDateTime(reader.GetOrdinal("deadline")).ToString("yyyy-MM-dd HH:mm:ss"),
                            JobTitle = reader.GetString(reader.GetOrdinal("jobTitle")),
                            Budget = reader.GetDecimal(reader.GetOrdinal("budget")),
                            JobOwnerName = reader.GetString(reader.GetOrdinal("jobOwnerName")),
                            JobOwerProfilePicLink = reader.GetString(reader.GetOrdinal("JobOwerProfilePicLink")),
                            IsWorkerAccept = reader.GetBoolean(reader.GetOrdinal("IsWorkerAccept")),
                            StatusDescription = reader.GetString(reader.GetOrdinal("statusDescription"))
                        };

                        jobDetailsForWorkerAll.JobDetailsForWorkerList.Add(jobDetailsForWorkerObj);
                    }

                    // Move to the next result set
                    if (await reader.NextResultAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            jobDetailsForWorkerAll.NumberOfJobsCompleted = reader.GetInt32(reader.GetOrdinal("NumberOfJobsCompleted"));
                            jobDetailsForWorkerAll.TotalEarnings = reader.GetDecimal(reader.GetOrdinal("TotalEarnings"));
                        }
                    }
                }

                return (jobDetailsForWorkerAll);
            }
            catch (Exception ex)
            {
                // Log the exception as necessary
                throw new Exception("Error retrieving job worker mode details", ex);
            }
        }



        public async Task<JobHiringModeResponse> GetJobDetailsHiringModeAsync(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
            new SqlParameter("@UserId", userId)
                };

                var response = new JobHiringModeResponse
                {
                    JobHiringModeDetails = new List<JobHiringModeDetail>(),
                    JobBeddingDetails = new List<JobBeddingDetail>()
                };

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[job].[GetJobDetailsForHiringMode]", parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        var jobHiringModeDetail = new JobHiringModeDetail
                        {
                            JobId = reader.GetInt32(reader.GetOrdinal("ID")),
                            PostedDate = reader.GetDateTime(reader.GetOrdinal("postedDate")).ToString("yyyy-MM-dd HH:mm:ss"),
                            Deadline = reader.GetDateTime(reader.GetOrdinal("deadline")).ToString("yyyy-MM-dd HH:mm:ss"),
                            JobTitle = reader.GetString(reader.GetOrdinal("jobTitle")),
                            ClientBudget = reader.GetDecimal(reader.GetOrdinal("budget")),
                            WorkerId = reader.GetInt32(reader.GetOrdinal("workerId")),
                            Status = reader.GetString(reader.GetOrdinal("statusDescription")),
                            StatusType = reader.GetInt32(reader.GetOrdinal("statusType"))
                        };
                        response.JobHiringModeDetails.Add(jobHiringModeDetail);
                    }

                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var jobBeddingDetail = new JobBeddingDetail
                            {
                                WorkerId = reader.GetInt32(reader.GetOrdinal("workerId")),
                                JobId = reader.GetInt32(reader.GetOrdinal("jobId")),
                                WokerProfileImageLink = reader.GetString(reader.GetOrdinal("profileImageLink")),
                                WorkerName = reader.GetString(reader.GetOrdinal("workerName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("price"))
                            };
                            response.JobBeddingDetails.Add(jobBeddingDetail);
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving job hiring mode details", ex);
            }
        }



        public async Task<bool> ChangeAcceptancyForPostedJobAsync(JobAcceptancyChangeForPostedJob jobAcceptancyChangeForPostedJob)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
            new SqlParameter("@WorkerId", SqlDbType.Int) { Value = jobAcceptancyChangeForPostedJob.WorkerId },
            new SqlParameter("@JobId", SqlDbType.Int) { Value = jobAcceptancyChangeForPostedJob.JobId },
            new SqlParameter("@IsAccept", SqlDbType.Bit) { Value = jobAcceptancyChangeForPostedJob.IsAccept }
                };

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[job].[ChangeAcceptancyForPostedJob]", parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        int affectedRows = reader.GetInt32(reader.GetOrdinal("AffectedRows"));
                        return affectedRows > 0;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error when changing the job acceptancy for posted job", ex);
            }
        }


        public async Task<bool> WorkerAcceptOrRejectJobAsync(JobAcceptancyByWorker jobAcceptancyByWorker)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
            new SqlParameter("@JobOwnerId", SqlDbType.Int) { Value = jobAcceptancyByWorker.JobOwnerId },
            new SqlParameter("@WorkerId", SqlDbType.Int) { Value = jobAcceptancyByWorker.WorkerId },
            new SqlParameter("@JobId", SqlDbType.Int) { Value = jobAcceptancyByWorker.JobId },
            new SqlParameter("@IsAccept", SqlDbType.Bit) { Value = jobAcceptancyByWorker.IsAccept }
                };

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[job].[WorkerAcceptOrRejectJob]", parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        int affectedRows = reader.GetInt32(reader.GetOrdinal("AffectedRows"));
                        return affectedRows > 0;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error when changing the job acceptancy for the posted job", ex);
            }
        }


        public async Task<bool> SaveNewJobAsync(JobPost job)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
            new SqlParameter("@JobTitle", job.JobTitle ?? (object)DBNull.Value),
            new SqlParameter("@JobCategory", job.JobCategory ?? (object)DBNull.Value),
            new SqlParameter("@Deadline", job.Deadline),
            new SqlParameter("@JobOwnerId", job.JobOwnerId),
            new SqlParameter("@DifficultyLevel", job.DifficultyLevel),
            new SqlParameter("@JobDescription", job.JobDescription ?? (object)DBNull.Value),
            new SqlParameter("@Attachment", job.Attachment ?? (object)DBNull.Value),
            new SqlParameter("@ClientExpectedBudget", job.ClientExpectedBudget)
                };


                var result = await _databaseService.ExecuteStoredProcedureWithReturnAsync("[job].[savePostJob]", parameters);

                return result == 1; // Return true if the insert was successful
            }
            catch (Exception ex)
            {
                throw new Exception("Error when saving the new job post", ex);
            }
        }









    }
}
