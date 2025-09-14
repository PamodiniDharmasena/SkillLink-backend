namespace SkillLink.BusinessObjects.JobActivityModule
{
    public class JobPost
    {
        public string JobTitle { get; set; }
        public string JobCategory { get; set; }
        public DateTime Deadline { get; set; }
        public int JobOwnerId { get; set; }
        public int DifficultyLevel { get; set;}
        public string JobDescription { get; set; }

        public decimal ClientExpectedBudget { get; set; }
        public string Attachment { get; set; }


    }
}
