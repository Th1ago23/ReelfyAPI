namespace Application.DTO.Content;

public record ContentsHomeDTO(int UserId, string Email, IEnumerable<ContentDTO> Contents)
{
}
