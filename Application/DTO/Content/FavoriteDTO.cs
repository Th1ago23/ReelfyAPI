namespace Application.DTO.Content
{
    public record FavoriteDTO(int userId, string email, ICollection<FavoriteContentDTO> movies)
    {
    }
}
