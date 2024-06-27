using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Donor()
        {
            return View();
        }

        public ActionResult Seeker()
        {
            return View();
        }

        public ActionResult Hospital()
        {
            return View();
        }

        public ActionResult BloodBank()
        {
            return View();
        }
    }
}