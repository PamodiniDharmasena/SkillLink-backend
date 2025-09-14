namespace SkillLink.BusinessObjects.JobActivityModule
{
    public class JobDetailsAsWorkerAll
    {
        public List<JobDetailsForWorker> JobDetailsForWorkerList { get; set; } = new List<JobDetailsForWorker>();
        public int NumberOfJobsCompleted { get; set; }
        public decimal TotalEarnings { get; set; }
    }
}
