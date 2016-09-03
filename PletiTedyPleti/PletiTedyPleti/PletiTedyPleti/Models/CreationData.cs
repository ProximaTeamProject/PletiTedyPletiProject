using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PletiTedyPleti.Models
{
    public class CreationData
    {
        public CreationData()
        {
            this.CreationTime = DateTime.Now;
        }

        [Key]
        public int CreationId { get; set; }

        public int CommentId { get; set; }

        public int PostId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}