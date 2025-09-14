namespace SkillLink.BusinessObjects.JobActivityModule
{
    public class JobHiringModeDetail
    {
        public int JobId { get; set; }
        public string PostedDate { get; set; }
        public string Deadline { get; set; }
        public string JobTitle { get; set; }
        public decimal ClientBudget { get; set; }
        public int WorkerId { get; set; }
        public string Status { get; set; }

        public int StatusType { get; set; }

        public List<JobBeddingDetail> JobBeddingInfoList { get; set; } = new List<JobBeddingDetail>();

    }
}
