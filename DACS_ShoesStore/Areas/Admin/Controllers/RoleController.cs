using DACS_ShoesStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACS_ShoesStore.Areas.Admin.Controllers
{
/*    [Authorize(Roles = "Admin")]
*/
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Role
        public ActionResult Index()
        {
            var item = db.Roles.ToList();
            return View(item);
        }

        //Create Role
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(IdentityRole model)
        {
            var rolorManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            rolorManager.Create(model);
            return RedirectToAction("Index");
        }

        // Edit Role
        public ActionResult Edit(int id )
        {   
            var item = db.Roles.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
            var rolorManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            rolorManager.Update(model);
            return RedirectToAction("Index");
            }
           return View(model);
        }

        //Delete Roles
        //Delete Role
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var role = db.Roles.Find(id);

            if (role == null)
            {
                return HttpNotFound();
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var result = roleManager.Delete(role);

            if (!result.Succeeded)
            {
                // Handle the delete error
                return View("Error");
            }

            return Json(new { success = true });
        }

    }
}