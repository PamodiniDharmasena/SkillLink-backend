
using SkillLink.BusinessObjects.ProfileModule;
using SkillLink.DataServices.ProfileModule;
using System.Data;

namespace SkillLink.ApplicationServices.ProfileModule
{
    public class ProfileApplicationService
    {

        private readonly ProfileDataService _profileDataService;

        public ProfileApplicationService(ProfileDataService profileDataService)
        {
            _profileDataService = profileDataService;
        }

        public async Task<ProfilePhotoLink> GetProfilePhoto(int UserId)
        {


            var profilePhotoLink = await _profileDataService.GetProfilePhotoAsync(UserId);
            return profilePhotoLink;


        }

        public async Task<ProfileInfo> GetProfileInfo(int UserId)
        {


            var profileInfo = await _profileDataService.GetProfileInfoAsync(UserId);
            return profileInfo;

           
        }
        

        public async Task<bool> UpdateBasicProfileInfo(EditBasicInfo editBasicInfo)
        {

            DataTable universities = new DataTable();
            universities.Columns.Add("Name", typeof(string));

            DataTable degrees = new DataTable();
            degrees.Columns.Add("Name", typeof(string));

            DataTable diplomas = new DataTable();
            diplomas.Columns.Add("Name", typeof(string));

            DataTable certificates = new DataTable();
            certificates.Columns.Add("Name", typeof(string));
            certificates.Columns.Add("IssueNumber", typeof(string));

            foreach (var item in editBasicInfo.University)
            {
                universities.Rows.Add(item);
            }

            foreach (var item in editBasicInfo.Degree)
            {
                degrees.Rows.Add(item);
            }

            foreach (var item in editBasicInfo.Diploma)
            {
                diplomas.Rows.Add(item);
            }

            foreach (var certificate in editBasicInfo.Certificate)
            {
                certificates.Rows.Add(certificate.CertificateName, certificate.CertificateIssueNo);
            }

            Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>
            {
                { "universities", universities },
                { "degrees", degrees },
                { "diplomas", diplomas },
                { "certificates", certificates }
            };

            var isUpdate = await _profileDataService.UpdateBasicProfileInfoAsync(editBasicInfo, tables);
            return isUpdate;


        }

    }
}
