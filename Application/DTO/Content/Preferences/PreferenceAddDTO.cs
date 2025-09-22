namespace Application.DTO.Content.Preferences
{
    public record PreferenceAddDTO(ICollection<CastAddDTO> castDTO, ICollection<CrewAddDTO> crewDTO, ICollection<GenreAddDTO> genreDTO, ICollection<StreamingAddDTO> streamingDTO)
    {
    }
}
