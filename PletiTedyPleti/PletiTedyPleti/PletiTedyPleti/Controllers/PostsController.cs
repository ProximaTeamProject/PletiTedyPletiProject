﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PletiTedyPleti.Models;
using PletiTedyPleti.Classes;

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

        public ActionResult LikeCounter(int Id)
        {
            var post = db.Posts.FirstOrDefault(x => x.Id == Id);
            post.LikeCounter++;
            db.SaveChanges();
          
            return RedirectToAction("Details", new { Id = Id });
        }
        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.FirstOrDefault(x => x.Id == id);


            if (post == null)
            {
                return HttpNotFound();
            }

            CategoriesViewCombination commentsViewCombination = new CategoriesViewCombination();

            var comments = db.Comments.Include(c => c.Posts).Include(x => x.Author).Where(y => y.PostId == id).ToList();
            var posts = new List<Post>();
            posts.Add(post);
            var images = db.Images.ToList();

            commentsViewCombination.CommentsCollection = comments;
            commentsViewCombination.PostsCollection = posts;
            commentsViewCombination.ImagesCollection = images;

            return View(commentsViewCombination);
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
        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            var category = db.Categories.ToList();

            ViewBag.Category = new SelectList(category, "Id", "name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrators")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Category,Title,Body,TagsRaw")] Post post)
        {
            if (ModelState.IsValid)
            {

                post.AddTagsToPost(db, post);
                post.Category = db.Categories.FirstOrDefault(x => x.Id.ToString() == post.Category).name;
                db.Posts.Add(post);
                db.SaveChanges();

                var category = db.Categories.ToList();

                ViewBag.Category = new SelectList(category, "name");
                return RedirectToAction("Index");
            }
            var category2 = db.Categories.ToList();

            ViewBag.Category = new SelectList(category2, "Id", "name");
            return View(post);
        }


        // GET: Posts/Edit/5
        [Authorize(Roles = "Administrators")]
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
        [Authorize(Roles = "Administrators")]
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
        [Authorize(Roles = "Administrators")]
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
        [Authorize(Roles = "Administrators")]
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
