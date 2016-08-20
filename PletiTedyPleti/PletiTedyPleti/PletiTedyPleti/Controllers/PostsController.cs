using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PletiTedyPleti.Models;

namespace PletiTedyPleti.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult DisplayTagSearchResults(int? id)
        {
            List<Post> searchResults = new List<Post>();

            foreach (var post in db.Posts.ToList())
            {
                foreach (var tag in post.Tags)
                {
                    if (tag.Id == id)
                    {
                        searchResults.Add(post);
                    }

                }

            }
            return View(searchResults);
        }



        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Category,Title,Body,Date,LikeCounter,TagsRaw")] Post post)
        {
            if (ModelState.IsValid)
            {
                List<Tag> tagsToAdd = DefineTags(db, post);

                foreach (var tag in tagsToAdd)
                {
                    post.Tags.Add(tag);
                }

                db.Posts.Add(post);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(post);
        }

        private List<Tag> DefineTags(ApplicationDbContext db, Post post)
        {
            List<string> tagsNames = post.TagsRaw.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

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

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category,Title,Body,Date,LikeCounter")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);

            post.Tags.Clear();

            var commentsToRemove = post.Comments;

            db.Comments.RemoveRange(commentsToRemove);

            post.Comments.Clear();

            db.Posts.Remove(post);

            db.SaveChanges();


            var TagsWithoutPosts = db.Tags.Where(x => x.Posts.Count == 0).ToList();

            db.Tags.RemoveRange(TagsWithoutPosts);


            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
