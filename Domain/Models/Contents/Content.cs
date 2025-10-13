using Domain.Models.Users;
using Domain.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Contents;

public class Content
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string? ContentType { get; set; }

    public ICollection<FavoriteContent> FavoritedByUsers { get; set; } = new List<FavoriteContent>();
    public ICollection<ContentsList> InUserContentLists { get; set; } = new List<ContentsList>();
    public ICollection<AlreadySeenContent> SeenInLists { get; set; } = new List<AlreadySeenContent>();

    public Content() { }

    public Content(int id, ICollection<FavoriteContent> user)
    {
        Id = id;
        FavoritedByUsers = user;

    }

}
