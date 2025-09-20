using Application.DTO.Content;
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
        [HttpPost("Preferences")]
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
    }
}
