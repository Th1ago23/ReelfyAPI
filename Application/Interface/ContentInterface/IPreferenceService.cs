using Application.DTO.Content;


namespace Application.Interface.ContentInterface;

public interface IPreferenceService
{
    public Task<PreferenceResponseDTO> Add(PreferenceAddDTO dto);
}
