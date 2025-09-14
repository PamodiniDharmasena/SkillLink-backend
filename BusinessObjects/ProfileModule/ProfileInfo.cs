namespace SkillLink.BusinessObjects.ProfileModule
{

    public class ProfileInfo
    {

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string MobileNumber { get; set; }
        public string Nic { get; set; }
        public DateTime BirthDay { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string School { get; set; }
        public string Job { get; set; }
        public int Experience { get; set; }


        public List<string> University { get; set; }
        public List<string> Diploma { get; set; }
        public List<string> Degree { get; set; }
        public List<string> CertificateDlt { get; set; }

        public List<CertificateFullInfo> CertificateList {get; set;}



    }

    public class CertificateFullInfo
    {
        public string CertificateName { get; set; }
        public string CertificateIssueNo { get; set; }

    }
}
