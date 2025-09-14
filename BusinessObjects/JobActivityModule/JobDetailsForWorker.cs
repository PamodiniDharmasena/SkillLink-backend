namespace SkillLink.BusinessObjects.JobActivityModule
{
    public class JobDetailsForWorker
    {
      public int ID { get; set; }
      public int JobOwnerId { get; set; }
      public string PostedDate { get; set; }
      public string Deadline { get; set; }
      public string JobTitle { get; set; }
      public decimal Budget { get; set; }
      public string JobOwnerName { get; set; }
      public string JobOwerProfilePicLink { get; set; }

      public bool IsWorkerAccept { get; set; }

      public string StatusDescription { get; set; }

    }
}
