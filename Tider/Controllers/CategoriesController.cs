using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
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
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index() {
            ViewBag.userImage = db.Users.Find(User.Identity.GetUserId()).Image_url;

            return View(db.Categories.ToList());
        }

        // GET: Categories/Posts/5
        public ActionResult Posts(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null) {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,Image_url")] Category category) {
            category.OpId = User.Identity.GetUserId();
            category.Date = DateTime.Now;

            if (ModelState.IsValid) {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Image_url")] Category category) {
            category.OpId = User.Identity.GetUserId();
            category.Date = DateTime.Now;

            if (ModelState.IsValid) {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
