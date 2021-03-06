﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PletiTedyPleti.Models;

namespace PletiTedyPleti.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {

            var posts = db.Posts.OrderByDescending(p => p.Category).Take(5);

            var mostPopularTags = db.Tags.Include(t=>t.Posts).OrderByDescending(x => x.Posts.Count).Take(5);

            var category = db.Categories;

            ViewBag.Tags = mostPopularTags;
            ViewBag.Categories = category;
            return View(posts.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}