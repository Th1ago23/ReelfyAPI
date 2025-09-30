using Application.DTO.Content.Preferences;
using Application.Interface.ContentInterface;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> AddPreferences(PreferenceAddDTO dto)
        {
            var serviceResponse = await _service.Add(dto);

            if (!serviceResponse.Success)
            {
                return StatusCode(serviceResponse.StatusCode, new { message = serviceResponse.Message });
            }

            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }

        [Authorize]
        [HttpGet("GetPreferences")]
        public async Task<IActionResult> GetPreferences()
        {
            var serviceResponse = await _service.GetAllPreferences();

            if (!serviceResponse.Success)
            {
                return StatusCode(serviceResponse.StatusCode, new { message = serviceResponse.Message });
            }

            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }
    }
}