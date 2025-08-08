using ReelfyAPI.Models.DTO;
using ReelfyAPI.Utils;
using System.ComponentModel.DataAnnotations;

namespace ReelfyAPI.Models
{
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
        public byte [] PasswordHash { get; set; }

        [Required]
        public byte [] PasswordSalt { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string? FavoriteContent {  get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


    }
}
