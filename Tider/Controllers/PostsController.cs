using Microsoft.AspNet.Identity;
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


            var posts = db.Posts.Include(p => p.Category).Include(p => p.Op).Where(p => p.CategoryId == categoryId);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null) {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create() {
            //ViewBag.CategoryId = new SelectList(db.Categories, "ID", "Title");
            //ViewBag.OpId = new SelectList(db.ApplicationUsers, "Id", "Nickname");
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Content,Image_url")] Post post) {
            post.OpId = User.Identity.GetUserId();
            post.Date = DateTime.Now;

            if (TempData.ContainsKey("categoryId")) {
                post.CategoryId = (int)TempData["categoryId"];
            } else return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid) {
                db.Posts.Add(post);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return Redirect("/Posts/Index/?categoryId=" + post.CategoryId);
            }

            //ViewBag.CategoryId = new SelectList(db.Categories, "ID", "Title", post.CategoryId);
            //ViewBag.OpId = new SelectList(db.ApplicationUsers, "Id", "Nickname", post.OpId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null) {
                return HttpNotFound();
            }
            //ViewBag.CategoryId = new SelectList(db.Categories, "ID", "Title", post.CategoryId);
            //ViewBag.OpId = new SelectList(db.ApplicationUsers, "Id", "Nickname", post.OpId);
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content,Image_url,Date,CategoryId,OpId")] Post post) {
            if (ModelState.IsValid) {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CategoryId = new SelectList(db.Categories, "ID", "Title", post.CategoryId);
            //ViewBag.OpId = new SelectList(db.ApplicationUsers, "Id", "Nickname", post.OpId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null) {
                return HttpNotFound();
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
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
