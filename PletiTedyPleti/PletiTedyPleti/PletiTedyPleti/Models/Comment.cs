using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;


namespace PletiTedyPleti.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Date = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public int PostId { get; set; }

        public ApplicationUser Author { get; set; }



        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public Post Posts { get; set; }

    }
}