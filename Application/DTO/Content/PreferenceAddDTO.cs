namespace Application.DTO.Content
{
    public record PreferenceAddDTO (ICollection<CastAddDTO> castDTO, ICollection<CrewAddDTO> crewDTO, ICollection<GenreAddDTO> genreDTO, ICollection<StreamingAddDTO> streamingDTO)
    {
    }
}
