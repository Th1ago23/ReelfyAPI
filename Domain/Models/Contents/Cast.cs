using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Contents;
public class Cast
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProfilePath { get; set; } = string.Empty;
    public ICollection<Preference> Preferences { get; set; } = new List<Preference>();

    public Cast()
    {
    }

    public Cast(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
