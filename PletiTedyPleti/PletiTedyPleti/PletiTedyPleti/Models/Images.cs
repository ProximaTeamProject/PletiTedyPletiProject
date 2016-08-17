using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PletiTedyPleti.Models
{
    public class Images
    {
        public Images()
        {
            this.Post = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string MainImg { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<Post> Post { get; set; }
        }
}