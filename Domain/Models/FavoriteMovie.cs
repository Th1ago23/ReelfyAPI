using Domain.Utils;
using ReelfyAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class FavoriteMovie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Title { get; set; }
        public Category category { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<User> User { get; set; } = new List<User>();


        public FavoriteMovie() { }

        public FavoriteMovie(int id, string Title, Category category, string ImageUrl, ICollection<User> user)
        {
            this.Id = id;
            this.Title = Title;
            this.category = category;
            this.ImageUrl = ImageUrl;
            this.User = user;

        }

    }
}
