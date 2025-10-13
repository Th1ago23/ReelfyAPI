namespace Application.DTO.Content;

public record ContentSeenAndFavoritedDTO(int ContentId, string ContentType, bool? IsFavorited, bool? IsSeen);