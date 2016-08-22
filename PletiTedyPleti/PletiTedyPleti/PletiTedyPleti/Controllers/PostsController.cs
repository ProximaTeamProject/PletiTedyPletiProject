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

    public class Combination
    {
        public IEnumerable<Comment> CommentsCollection { get; set; }
        public Post Post { get; set; }
    }

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

            Combination combinationModel = new Combination();

            List<Comment> commentsCollection = db.Comments.Where(x=>x.Posts.Id == post.Id).Include(y=>y.Author).ToList();

            combinationModel.CommentsCollection = commentsCollection;

            combinationModel.Post = post;

            return View(combinationModel);
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

                post.AddTagsToPost(db, post);

                db.Posts.Add(post);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(post);
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
        public ActionResult Edit([Bind(Include = "Id,Category,Title,Body,Date,LikeCounter,TagsRaw")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                post.AddTagsToPost(db, post);
                db.SaveChanges();

                CheckIfThereAreEmptyTagsAndRemoveThem();

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


            CheckIfThereAreEmptyTagsAndRemoveThem();


            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private void CheckIfThereAreEmptyTagsAndRemoveThem()
        {
            var TagsWithoutPosts = db.Tags.Where(x => x.Posts.Count == 0).ToList();

            db.Tags.RemoveRange(TagsWithoutPosts);
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
