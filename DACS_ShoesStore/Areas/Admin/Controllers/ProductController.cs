using DACS_ShoesStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Xunit.Abstractions;
using PagedList;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DACS_ShoesStore.Areas.Admin.Controllers
{
/*    [Authorize(Roles = "Admin")]
*/
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Product
        public ActionResult Index(int? id , int? page)
        {
            var item = db.Products.ToList();
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 5;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(item.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            var item = db.Categories.ToList();
            ViewBag.Loai = item;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model, List<string> Images, List<int> rDefault)
        {
            var item = db.Categories.ToList();
            ViewBag.Loai = item;
            return View();
        }

        [HttpPost]
        public ActionResult SaveProduct(Product product, HttpPostedFileBase FeatureImage)
        {
            if (!ModelState.IsValid)
            {
                var item = db.Categories.ToList();
                ViewBag.Loai = item;
                return View("Create", product);
            }

            if (FeatureImage != null && FeatureImage.ContentLength > 0)
            {
                string fileName = Path.GetFileName(FeatureImage.FileName);
                string path = Path.Combine(Server.MapPath("/Content/assets/img/product/"), fileName); // Add forward slash before "Content"
                FeatureImage.SaveAs(path);
                product.FeatureImage = "/Content/assets/img/product/" + fileName; // Add forward slash after "img/"
            }

            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        public ActionResult Edit(int id)
        {
            var listCate = db.Categories.ToList();
            ViewBag.Loai = listCate;
            var findProduct = db.Products.Find(id);
            return View(findProduct);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase FeatureImage)
        {
            var updateProduct = db.Products.Find(product.Id);
                
            if (FeatureImage != null && FeatureImage.ContentLength > 0)
            {
                var listCate = db.Categories.ToList();
                ViewBag.Loai = listCate;
/*                return View("Create", "Product");s
*/            }
            var Edit = db.Products.Find(product.Id);
            string path = Path.Combine(Server.MapPath("~/Content/assets/img/product/"), Path.GetFileName(FeatureImage.FileName));
            updateProduct.Title = product.Title;
            updateProduct.Price = product.Price;
            updateProduct.CategoryId = product.CategoryId;
            updateProduct.Des = product.Des;
            updateProduct.FeatureImage = product.FeatureImage;
            updateProduct.FeatureImage = "~/Content/assets/img/product/" + Path.GetFileName(FeatureImage.FileName);
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        public ActionResult Delete(int Id) //Delete Product
        {
            var findProduct = db.Products.Find(Id);
            db.Products.Remove(findProduct);
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}
