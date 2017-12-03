using FormAuth.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormAuth.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db = new ApplicationContext();
        public ActionResult Index(string catchall)
        {
            var cookie = new HttpCookie("test_cookie")
            {
                Value = DateTime.Now.ToString("dd.MM.yyyy"),
                Expires = DateTime.Now.AddMinutes(2),
            };
            Response.SetCookie(cookie);

            if (catchall != null)
            {
                ViewBag.catchall = catchall;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
        public ActionResult Price()
        {
            return View(db.Prices);
        }

        [HttpGet]
        public ActionResult EditPrice(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Price price = db.Prices.Find(id);
            if (price != null)
            {
                return View("Edit", price);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditPrice(Price price)
        {
            db.Entry(price).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Price");
        }

        // [HttpGet]
        public ActionResult Order(int? id)
        {

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}