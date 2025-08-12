using Domain.Utils;
using ReelfyAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int? FavoriteCount { get; set; }

        public ICollection<User> User { get; set; }

    }
}
