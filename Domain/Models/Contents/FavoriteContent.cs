
using Domain.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Contents;

public class FavoriteContent
{
    [Key]
    public int Id { get; set; }
    public string ContentType { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int ContentId { get; set; }
    public Content Content { get; set; }
}
