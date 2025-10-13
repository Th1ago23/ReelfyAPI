namespace Application.DTO.Content;

public record ListSummaryDTO(int ListId, string Name, string? Description, IEnumerable<ContentDTO> Contents);
