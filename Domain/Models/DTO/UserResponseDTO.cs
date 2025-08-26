namespace ReelfyAPI.Models.DTO
{
    public record UserResponseDTO(int Id,string name ,string Email, DateTime? CreatedAt)
    {
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
