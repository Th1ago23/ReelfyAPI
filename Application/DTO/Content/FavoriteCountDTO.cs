using Domain.Utils;

namespace Application.DTO.Content
{
    public record FavoriteCountDTO(string contentName,Category category,int contentId, int usersCount)
    {
    }
}
