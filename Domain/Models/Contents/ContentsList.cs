using Domain.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Contents;

public class ContentsList
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}
