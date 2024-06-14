using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class RegistrationController : Controller
    {
        public ActionResult UserRegistration()
        {
            var registration = new RegistrationMV();
            return View(registration); 
        }
    }
}