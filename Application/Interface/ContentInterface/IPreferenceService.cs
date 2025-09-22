using Application.DTO.Content.Preferences;


namespace Application.Interface.ContentInterface;

public interface IPreferenceService
{
    public Task<PreferenceResponseDTO> Add(PreferenceAddDTO dto);
    public Task<PreferenceResponseDTO> GetAllPreferences();
}
