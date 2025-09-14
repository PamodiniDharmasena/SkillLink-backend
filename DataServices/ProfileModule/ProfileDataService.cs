using SkillLink.BusinessObjects.ProfileModule;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillLink.DataServices.ProfileModule
{
    public class ProfileDataService
    {
        private readonly DatabaseService _databaseService;

        public ProfileDataService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<ProfilePhotoLink> GetProfilePhotoAsync(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserId", userId)
                };

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[profile].[GetProfilePhotoLinks]", parameters))
                {
                    var profilePhotoLink = new ProfilePhotoLink();

                    if (await reader.ReadAsync())
                    {
                        profilePhotoLink.ProfileImageLink = reader.GetString(reader.GetOrdinal("ProfileImageLink"));
                        profilePhotoLink.CoverImageLink = reader.GetString(reader.GetOrdinal("CoverImageLink"));
                    }

                    return profilePhotoLink;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching profile info: {ex.Message}", ex);
            }
        }

        public async Task<ProfileInfo> GetProfileInfoAsync(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserId", userId)
                };

                var profileDetails = new ProfileInfo
                {
                    University = new List<string>(),  
                    Diploma = new List<string>(),
                    Degree = new List<string>(),
                    CertificateDlt = new List<string>(),
                    CertificateList = new List<CertificateFullInfo>()
                };

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[profile].[GetProfileDetails]", parameters))
                {


                    if (await reader.ReadAsync())
                    {
                        profileDetails.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                        profileDetails.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        profileDetails.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        profileDetails.FullName = reader.GetString(reader.GetOrdinal("FullName"));
                        profileDetails.Address = reader.GetString(reader.GetOrdinal("UserAddress"));
                        profileDetails.Bio = reader.GetString(reader.GetOrdinal("Bio"));
                        profileDetails.MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber"));
                        profileDetails.Nic = reader.GetString(reader.GetOrdinal("Nic"));
                        profileDetails.BirthDay = reader.GetDateTime(reader.GetOrdinal("BirthDay"));
                        profileDetails.Title = reader.GetString(reader.GetOrdinal("Title"));
                        profileDetails.Country = reader.GetString(reader.GetOrdinal("Country"));
                        profileDetails.City = reader.GetString(reader.GetOrdinal("City"));
                        profileDetails.Location = reader.GetString(reader.GetOrdinal("CurrentLocation"));
                        profileDetails.School = reader.GetString(reader.GetOrdinal("School"));
                        profileDetails.Job = reader.GetString(reader.GetOrdinal("Job"));
                        profileDetails.Experience = reader.GetInt32(reader.GetOrdinal("Experience"));

                    }
                    

                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string UniName = reader.GetString(reader.GetOrdinal("uniName"));
                            profileDetails.University.Add(UniName);
                        }
                    }

                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string Degree = reader.GetString(reader.GetOrdinal("degreeName"));
                            profileDetails.Degree.Add(Degree);
                        }
                    }

                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string Diploma = reader.GetString(reader.GetOrdinal("diplomaName"));
                            profileDetails.Diploma.Add(Diploma);
                        }
                    }


                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var Certificate = new CertificateFullInfo();


                            Certificate.CertificateName = reader.GetString(reader.GetOrdinal("certificateName"));
                            Certificate.CertificateIssueNo = reader.GetString(reader.GetOrdinal("issueNumber"));
                            string CertificateDtl = reader.GetString(reader.GetOrdinal("certificateDtl"));
                            profileDetails.CertificateDlt.Add(CertificateDtl);
                            profileDetails.CertificateList.Add(Certificate);
                            

                        }
                    }



                    return profileDetails;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching profile info: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateBasicProfileInfoAsync(EditBasicInfo editBasicInfo, Dictionary<string, DataTable> dataTables)
        {
            try
            {

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserId", editBasicInfo.UserId),
                    new SqlParameter("@FirstName", editBasicInfo.FirstName),
                    new SqlParameter("@LastName", editBasicInfo.LastName),
                    new SqlParameter("@Country", editBasicInfo.Country),
                    new SqlParameter("@City", editBasicInfo.City),
                    new SqlParameter("@Address", editBasicInfo.Address),
                    new SqlParameter("@Title", editBasicInfo.Title),
                    new SqlParameter("@Job", editBasicInfo.Job),
                    new SqlParameter("@Experience", editBasicInfo.Experience),
                    new SqlParameter("@School", editBasicInfo.School),
                      new SqlParameter("@Bio", editBasicInfo.Bio),

                    new SqlParameter("@Universities", SqlDbType.Structured)
                    {
                        TypeName = "dbo.StringList",
                        Value = dataTables["universities"]
                    },
                    new SqlParameter("@Degrees", SqlDbType.Structured)
                    {
                        TypeName = "dbo.StringList",
                        Value = dataTables["degrees"]
                    },
                    new SqlParameter("@Diplomas", SqlDbType.Structured)
                    {
                        TypeName = "dbo.StringList",
                        Value = dataTables["diplomas"]
                    },
                    new SqlParameter("@Certificates", SqlDbType.Structured)
                    {
                        TypeName = "dbo.CertificateList",
                        Value = dataTables["certificates"]
                    }
                  
                    };

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[profile].[UpdateBasicProfileInfo]", parameters))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating profile info: {ex.Message}", ex);
            }
        }

    }
}
