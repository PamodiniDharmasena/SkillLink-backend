namespace SkillLink.BusinessObjects.JobActivityModule
{
    public class JobHiringModeResponse
    {
        public List<JobHiringModeDetail> JobHiringModeDetails { get; set; }
        public List<JobBeddingDetail> JobBeddingDetails { get; set; }

        public JobHiringModeResponse()
        {
            JobHiringModeDetails = new List<JobHiringModeDetail>();
            JobBeddingDetails = new List<JobBeddingDetail>();
        }
    }

}
