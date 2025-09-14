using Microsoft.AspNetCore.Mvc;
using SkillLink.ApplicationServices.ProfileModule;
using SkillLink.BusinessObjects.ProfileModule;
using System;
using System.Threading.Tasks;

namespace SkillLink.Controllers.ProfileModule
{
    [ApiController]
    [Route("api/v1.0/[controller]/[action]")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileApplicationService _profileApplicationService;

        public ProfileController(ProfileApplicationService profileApplicationService)
        {
            _profileApplicationService = profileApplicationService;
        }

        [HttpGet(Name = "GetProfilePhoto")]
        public async Task<IActionResult> GetProfilePhoto(int UserId)
        {
            try
            {
                var profilePhotoLink = await _profileApplicationService.GetProfilePhoto(UserId);
                return Ok(profilePhotoLink);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetProfileDetails")]
        public async Task<IActionResult> GetProfileDetails(int UserId)
        {
            try
            {
                var profileDetails = await _profileApplicationService.GetProfileInfo(UserId);
                return Ok(profileDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "UpdateBasicProfileDetails")]
        public async Task <IActionResult> UpdateBasicInfo([FromBody] EditBasicInfo editBasicInfo)
        {
            try
            {
                var isUpdate = await _profileApplicationService.UpdateBasicProfileInfo(editBasicInfo);
                return Ok(isUpdate);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
