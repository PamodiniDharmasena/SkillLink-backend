namespace SkillLink.BusinessObjects.JobActivityModule
{
    public class JobCard
    {
        public int ID { get; set; }
        public string JobTitle { get; set; }
        public string JobCategory { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime Deadline { get; set; }
        public int UserId { get; set; }
        public int LevelOfDifficulty { get; set; }
        public string JobDescription { get; set; }
        public string Attachments { get; set; }
        public decimal ClientEstimateBudget { get; set; }
        public string ProfileImageLink { get; set; }
        public decimal LoyaltyRatingAsClient { get; set; }

    }
}
