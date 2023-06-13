using DACS_ShoesStore.Models;
using Microsoft.Azure.Amqp.Framing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACS_ShoesStore.Areas.Admin.Controllers
{
/*    [Authorize(Roles = "Admin")]
*/    public class DashBoardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/DashBoard
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails on o.Id equals od.OderId
                        join p in db.Products on od.ProductId equals p.Id
                        select new
                        {
                            CreateDate = (DateTime?)o.CreatedDate, // Sử dụng kiểu dữ liệu nullable DateTime
                            Quantity = od.Quantity,
                            Price = od.Price
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreateDate >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreateDate < endDate);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreateDate))
                              .Select(x => new
                              {
                                  Date = x.Key,
                                  TotalSell = x.Sum(y => y.Quantity * y.Price)
                              })
                              .Select(x => new
                              {
                                  Date = x.Date,
                                  DoanhThu = x.TotalSell
                              });

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}