namespace SkillLink.BusinessObjects.JobActivityModule
{
    public class JobAcceptancyByWorker
    {
        public int JobOwnerId { get; set; }
        public int WorkerId { get; set; }
        public int JobId { get; set; }
        public bool IsAccept { get; set; }
    }
}
