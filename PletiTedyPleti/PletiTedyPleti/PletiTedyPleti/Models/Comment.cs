using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PletiTedyPleti.Models
{
    public class Comment
    {
        Comment()
        {
            this.Date = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public int PostId { get; set; }

        [StringLength(128)]
        public string AuthorId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Post Post { get; set; }
    }
}