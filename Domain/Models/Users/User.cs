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
    public DateOnly Birthday { get; set; }
    public bool IsPreemium { get; set; } = false;

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
    public Preference Preference { get; set; } = new Preference();

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool ValidateAge ()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var age = today.Year - Birthday.Year;

        if (Birthday > today.AddYears(-age)) age--;

        return age >= 16;
    
    }
    public int GetAge()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - Birthday.Year;

        if (Birthday > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }


}
