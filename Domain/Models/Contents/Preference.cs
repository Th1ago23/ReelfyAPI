using Domain.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Contents;

public class Preference
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
    public ICollection<Cast> Casts { get; set; } = new List<Cast>();
    public ICollection<Crew> Crews { get; set; } = new List<Crew>();
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public ICollection<Streaming> Streamings { get; set; } = new List<Streaming>();

}
