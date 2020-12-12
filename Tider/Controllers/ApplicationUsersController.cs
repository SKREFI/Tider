using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tider.Models;

namespace Tider.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();


        // GET: ApplicationUsers
        public ActionResult Index() {
            return View(db.Users.ToList());
        }

        // GET: ApplicationUsers/ProfilePage/5
        public ActionResult ProfilePage(string id) {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProfilePageViewModel viewModel = new ProfilePageViewModel {
                User = db.Users.Find(id),
                Posts = db.Posts.Where(p => p.OpId == id).ToList(),
            };
            viewModel.PostsCount = viewModel.Posts.Count();

            if (viewModel.User == null) return HttpNotFound();

            var userId = User.Identity.GetUserId();
            foreach (var item in db.UserRoles.Where(p => p.UserId == userId).ToList())
                Debug.WriteLine(db.Roles.Find(item.RoleId).Name);


            ViewBag.isAdmin = User.IsInRole(Const.ADMIN);
            ViewBag.isMod = User.IsInRole(Const.MODERATOR);

            return View(viewModel);
        }

        //POST: ApplicationUsers/EditRole/5/?action=add&?role=Moderator
        //POST: ApplicationUsers/EditRole/5/?action=revoke&?role=Admin
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole() {
            //var action = Request.QueryString["action"];
            string role = Request.QueryString["role"];
            string userId = Request.QueryString["userId"];

            Debug.WriteLine(role);
            Debug.WriteLine(userId);


            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            //UserManager.RemoveFromRoles(userId, new string[] { Const.ADMIN, Const.MODERATOR });

            UserManager.RemoveFromRole(userId, Const.ADMIN);
            UserManager.RemoveFromRole(userId, Const.MODERATOR);


            if (role == Const.ADMIN) UserManager.AddToRoles(userId, new string[] { Const.ADMIN, Const.MODERATOR });
            else if (role == Const.MODERATOR) UserManager.AddToRole(userId, Const.MODERATOR);
            else if (role != Const.USER) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Image_url,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser){
            if (ModelState.IsValid){
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id) {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
