using DACS_ShoesStore.Models;
using DACS_ShoesStore.Models.VNPay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACS_ShoesStore.Controllers
{
    public class CartShoppingController : Controller
    {
        // GET: CartShopping
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        [HttpPost]
public ActionResult AddToCart(int id, int quantity)
{
    var code = new { Success = false, msg = "", code = -1, Count = 0 };

    var db = new ApplicationDbContext();
    var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
    if (checkProduct != null)
    {
        ShoppingCart cart = (ShoppingCart)Session["Cart"];
        if (cart == null)
        {
            cart = new ShoppingCart();
        }
        ShoppingCartItem item = new ShoppingCartItem
        {
            ProductId = checkProduct.Id,
            ProductName = checkProduct.Title,
            Alias = checkProduct.Alias,
            Quantity = quantity

        };
        if (checkProduct.Category != null)
        {
            item.CategoryName = checkProduct.Category.Title;
        }
        if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
        {
            item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
        }
        item.Price = (decimal)checkProduct.Price;
        item.TotalPrice = item.Quantity * item.Price;
        cart.AddToCart(item, quantity);
        Session["Cart"] = cart;
        code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công", code = 1, Count = cart.Items.Count };
        return Json(code); // Trả về giá trị Json khi thành công
    }
    
    return Json(code); // Trả về giá trị Json khi không thành công
}

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        public ActionResult Checkout()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
/*------------------------CheckOut------------------------------------ */
        public ActionResult Partial_CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    order.Email = req.Email;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        Total = (int)(x.Price*x.Quantity),
                    }));
                    order.TotalAmount = (int)cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.ModifiedDate = DateTime.Now;
                    //order.CreatedBy = req.Phone;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    db.Orders.Add(order);
                    db.SaveChanges();
                    cart.ClearCart();
                    code = new { Success = true, Code = 1 };
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }

      
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }


        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

      

    }
}