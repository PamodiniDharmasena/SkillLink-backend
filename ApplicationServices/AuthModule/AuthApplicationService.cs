using SkillLink.BusinessObjects.AuthModule;
using SkillLink.DataServices.AuthModule;

namespace SkillLink.ApplicationServices.AuthModule
{
    public class AuthApplicationService
    {
        private readonly AuthDataService _authDataService;

        public AuthApplicationService(AuthDataService authDataService)
        {
            _authDataService = authDataService;
        }
        public async Task<LoginResult> CheckUserAuth(LoginModel loginModel)
        {
            var loginResult = await _authDataService.CheckUserAuthAsync(loginModel);
            return loginResult;
        }
    }
}
