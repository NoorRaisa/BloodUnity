using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class AccountsController : Controller
    {
        OnlineBloodBankDbEntities DB=new OnlineBloodBankDbEntities();
        public ActionResult AllNewUserRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var list = new List<RegistrationMV>();
            var users=DB.UserTables.Where(u=>u.AccountStatusID==1).ToList();

            return View(users);
        }
        public ActionResult UserDetails(int? id) 
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user=DB.UserTables.Find(id);

            return View(user); 
        }
        public ActionResult UserApproved(int? id) 
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 2;
            DB.Entry(user).State=System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequests"); 
        }
        public ActionResult UserRejected(int? id) 
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 3;
            DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequests");

        }
        public ActionResult AddNewDonorByBloodBank()
        {
            var CollectBloodMV = new CollectBloodMV();
            return View(CollectBloodMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewDonorByBloodBank(CollectBloodMV collectBloodMV )
        {
            return RedirectToAction("BloodBankStock", "BloodBank");
            ///return View(collectBloodMV);
        }
    }
}