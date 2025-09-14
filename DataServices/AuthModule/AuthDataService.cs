using Microsoft.AspNetCore.SignalR;
using SkillLink.BusinessObjects.AuthModule;
using SkillLink.BusinessObjects.JobActivityModule;
using System.Data.SqlClient;

namespace SkillLink.DataServices.AuthModule
{
    public class AuthDataService
    {

        private readonly DatabaseService _databaseService;

        public AuthDataService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<LoginResult> CheckUserAuthAsync(LoginModel loginModel)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@Email", loginModel.Email ?? (object)DBNull.Value),
                    new SqlParameter("@Password", loginModel.Password ?? (object)DBNull.Value),
                   
                };

                var loginResult = new LoginResult
                {
                    UserId = 0,
                    IsLoggedIn = false,
                };
                      
                   

                using (var reader = await _databaseService.ExecuteStoredProcedureAsync("[auth].[CheckUserLogin]", parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        loginResult.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));

                        if(loginResult.UserId != 0)
                        {
                            loginResult.IsLoggedIn = true;
                        }

                    }
                }

                return loginResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving job card details", ex);
            }
        }
    }
}
