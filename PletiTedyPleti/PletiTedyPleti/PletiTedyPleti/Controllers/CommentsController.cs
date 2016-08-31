using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PletiTedyPleti.Models;

namespace PletiTedyPleti.Controllers
{

    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index(int? id)
        {
            Combination commentsViewCombination = new Combination();

            var comments = db.Comments.Include(c => c.Posts).Include(x => x.Author).Where(y => y.PostId == id).ToList();
            Post post = db.Posts.FirstOrDefault(x => x.Id == id);

            commentsViewCombination.CommentsCollection = comments;
            commentsViewCombination.Post = post;

            return PartialView("_IndexPartial", commentsViewCombination);
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Include(y => y.Author).FirstOrDefault(x => x.Id == id);

            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            var post = db.Posts.Where(x => x.Id == id);

            ViewBag.PostId = new SelectList(post, "Id", "Title");
            ViewBag.PostIdNumber = post.FirstOrDefault().Id;
            return PartialView("_CreatePartial");
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Body,PostId")] Comment comment)
        {
            var postInACollection = db.Posts.Where(x => x.Id == comment.PostId);
            var postAsSinglePost = postInACollection.FirstOrDefault();

            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                comment.Author = currentUser;

                db.Comments.Add(comment);
                db.SaveChanges();

                db.CreationDataTable.Add(new CreationData()
                {
                    CommentId = comment.Id,
                    CreationTime = comment.Date,
                    PostId = comment.PostId
                });

                db.SaveChanges();

                ViewBag.PostId = new SelectList(postInACollection, "Id", "Title");

                ViewBag.Condition = true;

                Combination commentsViewCombination = new Combination();

                var comments = db.Comments.Include(c => c.Posts).Include(x => x.Author).Where(y => y.PostId == comment.PostId).ToList();

                commentsViewCombination.CommentsCollection = comments;
                commentsViewCombination.Post = postAsSinglePost;

                return PartialView("_CreatePartial");
            }

            ViewBag.PostId = new SelectList(postInACollection, "Id", "Title");
            return PartialView("_CreatePartial", comment);
        }

        // GET: Comments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Include(y => y.Author).FirstOrDefault(x => x.Id == id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            if (comment.Author.Id != User.Identity.GetUserId() && !User.IsInRole("Administrators"))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.PostId = new SelectList(db.Posts, "Id", "Category", comment.PostId);

            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Body,Date")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                comment.Author = currentUser;

                comment.TimeOfLastChange = DateTime.Now;
                comment.AuthorOfLastChangeName = currentUser.UserName;

                db.Entry(comment).State = EntityState.Modified;

                db.SaveChanges();

                comment.Date = db.CreationDataTable.FirstOrDefault(x => x.CommentId == comment.Id).CreationTime;
                comment.PostId = db.CreationDataTable.FirstOrDefault(x => x.CommentId == comment.Id).PostId;
                db.Entry(comment).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Category", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments.Include(y => y.Author).FirstOrDefault(x => x.Id == id);

            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Posts", new { id = comment.PostId });

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
