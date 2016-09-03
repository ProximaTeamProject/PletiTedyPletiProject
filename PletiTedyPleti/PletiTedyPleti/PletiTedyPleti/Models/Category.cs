using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PletiTedyPleti.Models
{
    public class Category
    {
        public Category()
        {
            Post = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string adress { get; set; }

        public virtual ICollection<Post> Post { get; set; }

    }
}