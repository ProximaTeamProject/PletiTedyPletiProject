using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PletiTedyPleti.Models;

namespace PletiTedyPleti.Classes
{
    public class CategoriesViewCombination
    {
        public IEnumerable<Images> ImagesCollection { get; set; }
        public IEnumerable<Post> PostsCollection { get; set; }

        public IEnumerable<Comment> CommentsCollection { get; set; }

        public Category Category { get; set; }
    }
}