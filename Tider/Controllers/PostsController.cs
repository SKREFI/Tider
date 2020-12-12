﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tider.Models;

namespace Tider.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index(int? categoryId) {
            TempData["categoryId"] = categoryId;
            ViewBag.categoryId = categoryId;

            if (categoryId == null) {
                ViewBag.Title = "Hot Page - Not done yet";
                return View(new List<Post>());
            }

            ViewBag.UserImage = db.Users.Find(User.Identity.GetUserId()).Image_url;
            var posts = db.Posts.Include(p => p.Category).Include(p => p.Op).Where(p => p.CategoryId == categoryId).ToList();
            return View(posts);
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content,Image_url")] Post post) {
            post.OpId = User.Identity.GetUserId();
            post.Date = DateTime.Now;

            if (TempData.ContainsKey("categoryId")) post.CategoryId = (int)TempData["categoryId"];
            else return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid) {
                db.Posts.Add(post);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }

            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,Image_url")] Post post) {
            post.Date = DateTime.Now;
            post.OpId = User.Identity.GetUserId();
            
            // TODO: Ask, why is post.CategoryId lost?????

            if (TempData.ContainsKey("categoryId")) {
                post.CategoryId = (int)TempData["categoryId"];
            } else return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid) {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();

            int categoryId;
            if (TempData.ContainsKey("categoryId")) {
                categoryId = (int) TempData["categoryId"];
            } else return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
