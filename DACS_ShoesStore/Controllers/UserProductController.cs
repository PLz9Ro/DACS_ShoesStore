using DACS_ShoesStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACS_ShoesStore.Controllers
{
    public class UserProductController : Controller
    {
        // GET: UserProduct
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult ProductDetails(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }

            return View(item);
        }

        public ActionResult Partial_ItemsNike()
        {
            var items = db.Products.ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ItemsAdidas()
        {
            var items = db.Products.ToList();
            return PartialView(items);
        }


    }
}
