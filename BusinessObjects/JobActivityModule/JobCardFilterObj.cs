namespace SkillLink.BusinessObjects.JobActivityModule
{
    public class JobCardFilterObj
    {
        public List<string>? Category { get; set; } = null;     
        public DateTime? Deadline { get; set; } = null;   
        public decimal? BudgetStart { get; set; } = null; 
        public decimal? BudgetEnd { get; set; } = null;   
        public int? DifficultyStart { get; set; } = null; 
        public int? DifficultyEnd { get; set; } = null;   
        public int? NumberOfViewsStart { get; set; } = null; 
        public int? NumberOfViewsEnd { get; set; } = null;
        public int UserId { get; set; }
    }
}
