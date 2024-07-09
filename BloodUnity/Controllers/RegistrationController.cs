using BloodUnity.Models;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
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
            if (ModelState.IsValid)
            {
                var checker = DB.UserTables.Where(u => u.UserName == registrationMV.User.UserName).FirstOrDefault();
                registrationmv = registrationMV;
                if (checker != null)
                {
                    ModelState.AddModelError(string.Empty, "Username Already Exists!");
                    return RedirectToAction("AlreadyExists", "Registration");
                }
                if (registrationMV.UserTypeID == 2 && registrationMV.ContactNo != null && registrationMV.User.Description != null && registrationMV.CityID.ToString() != null
                    && registrationMV.User.UserName != null && registrationMV.User.EmailAddress != null && registrationMV.User.Password != null && registrationMV.User.Password.Length >= 5)
                {
                    return RedirectToAction("DonorUser");
                }

                else if (registrationMV.UserTypeID == 3 && registrationMV.ContactNo != null && registrationMV.User.Description != null && registrationMV.CityID.ToString() != null
                    && registrationMV.User.UserName != null && registrationMV.User.EmailAddress != null && registrationMV.User.Password != null && registrationMV.User.Password.Length >= 5)
                {
                    return RedirectToAction("SeekerUser");
                }

                else if (registrationMV.UserTypeID == 4 && registrationMV.ContactNo != null && registrationMV.User.Description != null && registrationMV.CityID.ToString() != null
                    && registrationMV.User.UserName != null && registrationMV.User.EmailAddress != null && registrationMV.User.Password != null && registrationMV.User.Password.Length >= 5)
                    return RedirectToAction("HospitalUser");

                else if (registrationMV.UserTypeID == 5 && registrationMV.ContactNo != null && registrationMV.User.Description != null && registrationMV.CityID.ToString() != null
                    && registrationMV.User.UserName != null && registrationMV.User.EmailAddress != null && registrationMV.User.Password != null && registrationMV.User.Password.Length >= 5)
                    return RedirectToAction("BloodBankUser");
                else
                {
                    ///ModelState.AddModelError(string.Empty, "Please fill all the fields!");
                    return RedirectToAction("MainHome", "Home");
                }
            }

            return View(registrationMV);
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
                if (checktitle == null)
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
                            hospital.Email = registrationMV.User.EmailAddress;
                            hospital.Location = registrationMV.Hospital.Address;
                            hospital.CityID = registrationMV.CityID;
                            hospital.UserID = user.UserID;
                            DB.HospitalTables.Add(hospital);

                            transaction.Commit();
                            ViewData["Message"] = "Thanks for Registratiom, Your Query will be Reviewed Shortly!";
                            ModelState.AddModelError(string.Empty, "Your Account is Under Review. Please Login after two days!");
                            ///return RedirectToAction("MainHome", "Home");
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
            ViewData["Message"] = "Redirecting to Registration Page";
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(registrationmv);
            /*ViewBag.UserTypeID = new SelectList(DB.UserTypeTables.Where(ut => ut.UserTypeID > 1).ToList(), "UserTypeID", "UserType", registrationmv.UserTypeID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View(registrationmv);*/
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DonorUser(RegistrationMV registrationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.DonorTables.Where(h => h.FullName == registrationMV.Donor.FullName.Trim() && h.NID == registrationMV.Donor.NID).FirstOrDefault(); ///if title is registered or not
                if (checktitle == null)
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
                            //DB.SaveChanges();

                            var Donor = new DonorTable();
                            Donor.FullName = registrationMV.Donor.FullName;
                            Donor.BloodGroupID = registrationMV.BloodGroupID;
                            Donor.Location = registrationMV.Donor.Location;
                            Donor.ContactNo = registrationMV.ContactNo;
                            Donor.LastDonationDate = registrationMV.Donor.LastDonationDate;
                            Donor.NID = registrationMV.Donor.NID;
                            Donor.GenderID = registrationMV.GenderID;
                            Donor.CityID = registrationMV.CityID;
                            Donor.UserID = user.UserID;
                            DB.DonorTables.Add(Donor);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Your Account is Under Review. Please Login after two days!";
                            ///return RedirectToAction("MainHome", "Home");
                            ///ModelState.AddModelError(string.Empty, "Your Account is Under Review. Please Login after two days!");
                        }
                        catch (DbEntityValidationException e)
                        {
                            Console.WriteLine(e);
                            ModelState.AddModelError(string.Empty, "Please Provide Correct Information!");
                            transaction.Rollback();
                        }
                        catch (DbUpdateException e)
                        {
                            Console.WriteLine(e);
                            ModelState.AddModelError(string.Empty, "Please Provide Correct Information!");
                            transaction.Rollback();
                        }

                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Donor Already Registered!");
                }
            }
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", registrationMV.BloodGroupID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", registrationMV.GenderID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);
            /*
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View();*/
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
            if (ModelState.IsValid)
            {
                var checktitle = DB.BloodBankTables.Where(h => h.BloodBankName == registrationMV.BloodBank.BloodBankName.Trim() && h.PhoneNo == registrationmv.BloodBank.PhoneNo).FirstOrDefault(); ///if title is registered or not
                if (checktitle == null)
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
                            DB.SaveChanges();

                            var bloodBank = new BloodBankTable();
                            bloodBank.BloodBankName = registrationMV.BloodBank.BloodBankName;
                            bloodBank.Address = registrationMV.BloodBank.Location;
                            bloodBank.Location = registrationMV.BloodBank.Location;
                            bloodBank.PhoneNo = registrationMV.ContactNo;
                            bloodBank.Website = registrationMV.BloodBank.Website;
                            bloodBank.Email = registrationMV.User.EmailAddress;
                            bloodBank.CItyID = registrationMV.CityID;
                            bloodBank.UserID = user.UserID;
                            DB.BloodBankTables.Add(bloodBank);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thanks for Registratiom, Your Query will be Reviewed Shortly!";
                            ModelState.AddModelError(string.Empty, "Your Account is Under Review. Please Login after two days!");
                            ///return RedirectToAction("MainHome", "Home");
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
                    ModelState.AddModelError(string.Empty, "Blood Bank Already Registered!");
                }
            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);
        }
        public ActionResult SeekerUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");

            return View(registrationmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeekerUser(RegistrationMV registrationMV)
        {

            if (ModelState.IsValid)
            {
                var checktitle = DB.SeekerTables.Where(h => h.FullName == registrationMV.Seeker.FullName.Trim() && h.NID == registrationMV.Donor.NID).FirstOrDefault(); ///if title is registered or not
                if (checktitle == null)
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
                            //DB.SaveChanges();

                            var seeker = new SeekerTable();
                            seeker.FullName = registrationMV.Seeker.FullName;
                            seeker.Age = registrationMV.Seeker.Age;
                            seeker.BloodGroupID = registrationMV.BloodGroupID;
                            seeker.Address = registrationMV.Seeker.Address;
                            seeker.ContactNo = registrationMV.ContactNo;
                            seeker.RegistrationDate = DateTime.Now;
                            seeker.NID = registrationMV.Seeker.NID;
                            seeker.GenderID = registrationMV.GenderID;
                            seeker.CityID = registrationMV.CityID;
                            seeker.UserID = user.UserID;
                            DB.SeekerTables.Add(seeker);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thanks for Registration, Your Query will be Reviewed Shortly!";
                            ModelState.AddModelError(string.Empty, "Your Account is Under Review. Please Login after two days!");
                            ///return RedirectToAction("MainHome", "Home");
                        }
                        catch (DbEntityValidationException e)
                        {
                            Console.WriteLine(e);
                            ModelState.AddModelError(string.Empty, "Please Provide Correct Information!");
                            transaction.Rollback();
                        }
                        catch (DbUpdateException e)
                        {
                            Console.WriteLine(e);
                            ModelState.AddModelError(string.Empty, "Please Provide Correct Information!");
                            transaction.Rollback();
                        }

                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Seeker Already Registered!");
                }
            }
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", registrationMV.BloodGroupID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", registrationMV.GenderID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);
        }
        public ActionResult AlreadyExists()
        {
            return View();
        }
    }
}


