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
        [StringLength(200)]
        public string TagsRaw { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int LikeCounter { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<Images> Images { get; set; }

        public void AddTagsToPost(ApplicationDbContext db, Post post)
        {
            ClearAllPresentTags(db, post);

            List<string> tagsNames = post.TagsRaw.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            List<Tag> tagsToAdd = DefineTags(db, tagsNames);

            AttachTagsToPost(post, tagsToAdd);
        }

        private static void AttachTagsToPost(Post post, List<Tag> tagsToAdd)
        {
            foreach (var tag in tagsToAdd)
            {
                tag.Posts.Add(post);
                post.Tags.Add(tag);
            }
        }

        private static List<Tag> DefineTags(ApplicationDbContext db, List<string> tagsNames)
        {
            List<Tag> tagsCollection = new List<Tag>();

            foreach (var element in tagsNames)
            {
                Tag newTag;

                if (db.Tags.Any(x => x.Name == element))
                {
                    newTag = db.Tags.FirstOrDefault(x => x.Name == element);
                }
                else
                {
                    newTag = new Tag()
                    {
                        Name = element,
                    };

                    db.Tags.Add(newTag);

                    db.SaveChanges();
                }


                tagsCollection.Add(newTag);
            }

            return tagsCollection;
        }

        private void ClearAllPresentTags(ApplicationDbContext db, Post post)
        {
            post.Tags.Clear();

            if (db.Tags.Any(x => x.Posts.Any(y => y.Id == this.Id)))
            {
                var tag = db.Tags.FirstOrDefault(x => x.Posts.Any(y => y.Id == this.Id));

                tag.Posts.Remove(this);
            }
        }
    }
}