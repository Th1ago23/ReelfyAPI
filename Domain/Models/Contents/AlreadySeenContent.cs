using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Contents
{
    public class AlreadySeenContent
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int ContentId { get; set; }
        public Content Content { get; set; }
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    }
}
