using Application.DTO.Content.Preferences;
using Application.Interface.ContentInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReelfyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenceController : ControllerBase
    {
        private readonly IPreferenceService _service;

        public PreferenceController(IPreferenceService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("AddPreferences")]
        public async Task<IActionResult> AddPreferences (PreferenceAddDTO dto)
        {
            try
            {
                var preference = await _service.Add(dto);
                return Ok(preference);

            }catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e);
            }
        }
        [Authorize]
        [HttpGet("GetPreferences")]
        public async Task<IActionResult> GetPreferences()
        {
            try
            {
                var result = await _service.GetAllPreferences();

                return Ok(result);
            }catch(NullReferenceException e)
            {
                return BadRequest(e.Message);
            }
            catch(UnauthorizedAccessException e) 
            {
                return Unauthorized(e.Message);
            }
        }
    }
}
