using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PletiTedyPleti.Models
{
    public class Change
    {
        public Change()
        {
        }

        [Key]
        public int Key { get; set; }

        public DateTime TimeOfChange { get; set; }
        public ApplicationUser Author { get; set; }

        public override string ToString()
        {
            string change = $"Редактирано на {this.TimeOfChange} от {this.Author.UserName}";

            return change;
        }
    }
}