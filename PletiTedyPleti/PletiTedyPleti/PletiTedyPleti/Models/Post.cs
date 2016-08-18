using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PletiTedyPleti.Models
{
    public class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Images = new HashSet<Images>();
            this.Tags = new HashSet<Tag>();
            this.Tags.Add(new Tag());
            this.Tags.FirstOrDefault().Name = "TheBest";
            this.Date = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Category { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        public int LikeCounter { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<Images> Images { get; set; }
    }
}