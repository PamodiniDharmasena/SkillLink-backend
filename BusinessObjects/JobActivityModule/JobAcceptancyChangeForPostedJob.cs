namespace SkillLink.BusinessObjects.JobActivityModule
{
    public class JobAcceptancyChangeForPostedJob
    {
        public int WorkerId { get; set; }
        public int JobId { get; set; }
        public Boolean IsAccept { get; set; }
    }
}
