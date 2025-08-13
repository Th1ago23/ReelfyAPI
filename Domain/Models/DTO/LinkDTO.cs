namespace ReelfyAPI.Models.DTO
{
    public record LinkDTO(string Href, string Rel, string Method, string? Title = null, string? Type = null);
}
