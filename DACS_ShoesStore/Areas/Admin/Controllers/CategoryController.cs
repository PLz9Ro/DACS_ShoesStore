using DACS_ShoesStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACS_ShoesStore.Areas.Admin.Controllers
{
/*        [Authorize(Roles = "Admin")]
*/    public class CategoryController : Controller
    {

        // GET: Admin/Category
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var item = db.Categories.ToList();
            return View(item);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            { 
                model.Alias = DACS_ShoesStore.Models.Common.Filter.FilterChar(model.Title);
                db.Categories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        //Category/Edit

        public ActionResult Edit(int id)
        {
            var item = db.Categories.Find(id);
            return View(item);
        }
        //Category/Delete

        public ActionResult Delete(int id)
        {
            var findCategory = db.Categories.Find(id);

            if (findCategory == null)
            {
                return HttpNotFound();
            }

            // Find and delete the associated products
            var productsToDelete = db.Products.Where(p => p.CategoryId == id);
            db.Products.RemoveRange(productsToDelete);

            // Delete the category
            db.Categories.Remove(findCategory);

            db.SaveChanges();
            return RedirectToAction("Index", "Category");
        }


        /*
                public ActionResult Delete(int id)
                {
                    var findProduct = db.Categories.Find(id);
                    db.Categories.Delete(findProduct);
                    db.SaveChanges();
                }*/
    }
}