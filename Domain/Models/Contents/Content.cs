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
    public string? Title { get; set; }
    public Category? category { get; set; }
    public string? ImageUrl { get; set; }

    public ICollection<FavoriteContent> FavoritedByUsers { get; set; } = new List<FavoriteContent>();
    public ICollection<ContentsList> InUserContentLists { get; set; } = new List<ContentsList>();

    public Content() { }

    public Content(int id, string Title, Category category, string ImageUrl, ICollection<FavoriteContent> user)
    {
        Id = id;
        this.Title = Title;
        this.category = category;
        this.ImageUrl = ImageUrl;
        FavoritedByUsers = user;

    }

}
