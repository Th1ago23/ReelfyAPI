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
    public string Title { get; set; }
    public Category category { get; set; }
    public string ImageUrl { get; set; }
    public bool AlreadySeen { get; set; } = false;

    public ICollection<User> User { get; set; } = new List<User>();

    public Content() { }

    public Content(int id, string Title, Category category, string ImageUrl, ICollection<User> user)
    {
        Id = id;
        this.Title = Title;
        this.category = category;
        this.ImageUrl = ImageUrl;
        User = user;

    }

}
