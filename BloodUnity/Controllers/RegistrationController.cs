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

        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        static RegistrationMV registrationmv;
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectUser(RegistrationMV registrationMV)
        {
            //ViewBag.msg = "";
            registrationmv= registrationMV;
            if (registrationMV.UserTypeID == 2 && registrationMV.ContactNo!= null && registrationMV.User.Description!=null && registrationMV.CityID.ToString()!=null 
                && registrationMV.User.UserName != null && registrationMV.User.EmailAddress != null && registrationMV.User.Password != null)
            {
                return RedirectToAction("DonorUser");
            }
            
            else if (registrationMV.UserTypeID == 3 && registrationMV.ContactNo != null && registrationMV.User.Description != null && registrationMV.CityID.ToString() != null
                && registrationMV.User.UserName != null && registrationMV.User.EmailAddress != null && registrationMV.User.Password != null)
                return RedirectToAction("SeekerUser");

            else if (registrationMV.UserTypeID == 4 && registrationMV.ContactNo != null && registrationMV.User.Description != null && registrationMV.CityID.ToString() != null
                && registrationMV.User.UserName != null && registrationMV.User.EmailAddress != null && registrationMV.User.Password != null)
                return RedirectToAction("HospitalUser");

            else if (registrationMV.UserTypeID == 5 && registrationMV.ContactNo != null && registrationMV.User.Description != null && registrationMV.CityID.ToString() != null
                && registrationMV.User.UserName != null && registrationMV.User.EmailAddress != null && registrationMV.User.Password != null)
                return RedirectToAction("BloodBankUser");
            else
            {
                ///ModelState.AddModelError(string.Empty, "Please fill all the fields!");
                return RedirectToAction("MainHome", "Home");
            }
            //var registration = new RegistrationMV();
            //return View(registrationmv); 
        }
        public ActionResult HospitalUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View(registrationmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HospitalUser(RegistrationMV registrationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.HospitalTables.Where(h => h.FullName == registrationMV.Hospital.FullName.Trim()).FirstOrDefault(); ///if title is registered or not
                if(checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registrationMV.User.UserName;
                            user.Password = registrationMV.User.Password;
                            user.EmailAddress = registrationMV.User.EmailAddress;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registrationMV.UserTypeID;
                            user.Description = registrationMV.User.Description;
                            DB.UserTables.Add(user);

                            var hospital = new HospitalTable();
                            hospital.FullName = registrationMV.Hospital.FullName;
                            hospital.Address = registrationMV.Hospital.Address;
                            hospital.PhoneNo = registrationMV.ContactNo;
                            hospital.Website = registrationMV.Hospital.Website;
                            hospital.Email = registrationMV.Hospital.Email;
                            hospital.Location = registrationMV.Hospital.Address;
                            hospital.CityID = registrationMV.CityID;
                            hospital.UserID = user.UserID;
                            DB.HospitalTables.Add(hospital);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thanks for Registratiom, Your Query will be Reviewed Shortly!";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch 
                        {
                            ModelState.AddModelError(string.Empty, "Please Provide Correct Information!");
                            transaction.Rollback();
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Hospital Already Registered!");
                }
            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);
        }

        public ActionResult DonorUser()
        {
            ViewBag.UserTypeID = new SelectList(DB.UserTypeTables.Where(ut => ut.UserTypeID > 1).ToList(), "UserTypeID", "UserType", registrationmv.UserTypeID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View(registrationmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DonorUser(RegistrationMV registrationMV)
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View();
        }
        public ActionResult BloodBankUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            ///return View(registration);
            return View(registrationmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BloodBankUser(RegistrationMV registrationMV)
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View();
        }
        public ActionResult SeekerUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View(registrationmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeekerUser(RegistrationMV registrationMV)
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View();
        }
    }
}