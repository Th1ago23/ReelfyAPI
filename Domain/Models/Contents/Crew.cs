using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Contents;
public class Crew
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Preference> Preferences { get; set; } = new List<Preference>();

    public Crew(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
