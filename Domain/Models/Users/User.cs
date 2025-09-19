using Domain.Models.Contents;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Users;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int Age { get; set; }

    [Required]
    [MaxLength(90)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    public ICollection<Content> Contents { get; set; } = new List<Content>();
    public int PreferenceId { get; set; }
    public Preference Preference { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }


}
