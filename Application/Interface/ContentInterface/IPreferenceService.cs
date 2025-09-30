using Application.DTO.Content.Preferences;
using ReelfyAPI.Models;


namespace Application.Interface.ContentInterface;

public interface IPreferenceService
{
    public Task<Response<PreferenceResponseDTO>> Add(PreferenceAddDTO dto);
    public Task<Response<PreferenceResponseDTO>> GetAllPreferences();
}
