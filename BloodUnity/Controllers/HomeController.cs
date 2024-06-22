using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class HomeController : Controller
    {
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        public ActionResult Index()
        {
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
        public ActionResult MainHome()
        {
            var message = ViewData["Message"] == null ? "Welcome to Blood Unity" : ViewData["Message"];
            ViewData["Message"] = message;
            var registration = new RegistrationMV();
            ViewBag.UserTypeID = new SelectList(DB.UserTypeTables.Where(ut=>ut.UserTypeID>1).ToList(),"UserTypeID","UserType","0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            return View(registration);
        }
        public ActionResult Login()
        {
            var usermv=new UserMV();
            return View(usermv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserMV userMV)
        {
            return View();
        }

    }
}