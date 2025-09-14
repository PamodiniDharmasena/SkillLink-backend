namespace SkillLink.BusinessObjects.ProfileModule
{
    public class EditBasicInfo
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Job { get; set; }
        public int Experience { get; set; }
        public string School { get; set; }

        public List<string> University { get; set; } 
        public List<string> Degree { get; set; }
        public List<string> Diploma { get; set; }
        public List<CertificateFullInfo> Certificate { get; set; }

        public string Bio { get; set; }

    }
}
