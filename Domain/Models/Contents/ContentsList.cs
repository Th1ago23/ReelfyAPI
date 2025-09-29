using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Contents;

public class ContentsList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}
