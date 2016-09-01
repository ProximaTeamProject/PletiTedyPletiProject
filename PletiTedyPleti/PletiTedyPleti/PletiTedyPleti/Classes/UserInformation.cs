using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PletiTedyPleti.Models;

namespace PletiTedyPleti.Classes
{
    public class UserInformation
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<Post> CommentedPostsCollection { get; set; }
        public int CommentsCount { get; set; }
    }
}