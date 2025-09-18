using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Contents;

public class Genre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Preference> Preferences { get; set; } = new List<Preference>();

    public Genre(int id, string name, ICollection<Preference> preferences)
    {
        Id = id;
        Name = name;
        Preferences = preferences;
    }
}
