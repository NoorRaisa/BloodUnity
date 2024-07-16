using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class HomeController : Controller
    {
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        public ActionResult AllCampaigns()
        {
            var date = DateTime.Now.Date;
            var allcampaigns = DB.CampaignTables.Where(c=>c.CampaignDate>=date).ToList();
            return View(allcampaigns);
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

            var date = DateTime.Now.Date;
            var allcampaigns = DB.CampaignTables.Where(c => c.CampaignDate >= date).ToList();
            ViewBag.AllCampaigns = allcampaigns;

            var registration = new RegistrationMV();
            ViewBag.UserTypeID = new SelectList(DB.UserTypeTables.Where(ut => ut.UserTypeID > 1).ToList(), "UserTypeID", "UserType", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            return View(registration);
        }
        public ActionResult Login()
        {
            var usermv = new UserMV();
            return View(usermv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserMV userMV)
        {
            if (ModelState.IsValid)
            {
                var user = DB.UserTables.Where(u => u.Password == userMV.Password && u.UserName == userMV.UserName).FirstOrDefault();
                if (user != null)
                {
                    if (user.AccountStatusID == 1)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account is Under Review!");
                    }
                    else if (user.AccountStatusID == 3)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account is Rejected! For more Details Contact Us");
                    }
                    else if (user.AccountStatusID == 5)
                    {
                        ModelState.AddModelError(string.Empty, "Your Account is Suspended! For more Details Contact Us");
                    }
                    else if (user.AccountStatusID == 2)
                    {
                        Session["UserID"] = user.UserID;
                        Session["UserName"] = user.UserName;
                        Session["Password"] = user.Password;
                        Session["EmailAddress"] = user.EmailAddress;
                        Session["AccountStatusID"] = user.AccountStatusID;
                        Session["AccountStatus"] = user.AccountStatusTable.AccountStatus;
                        Session["UserTypeID"] = user.UserTypeID;
                        Session["UserType"] = user.UserTypeTable.UserType;
                        Session["Description"] = user.Description;

                        if (user.UserTypeID == 1)//admin
                        {
                            return RedirectToAction("AllNewUserRequests","Accounts");
                        }
                        else if (user.UserTypeID == 2)//donor
                        {
                            var donor = DB.DonorTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                            if (donor != null)
                            {
                                Session["DonorID"] = donor.DonorID;
                                Session["FullName"] = donor.FullName;
                                Session["GenderID"] = donor.GenderID;
                                Session["BloodGroupID"] = donor.BloodGroupID;
                                Session["BloodGroup"] = donor.BloodGroupsTable.BloodGroup;
                                Session["LastDonationDate"] = donor.LastDonationDate;
                                Session["ContactNo"] = donor.ContactNo;
                                Session["NID"] = donor.NID;
                                Session["Location"] = donor.Location;
                                Session["CityID"] = donor.CityID;
                                Session["City"] = donor.CityTable.City;
                                return RedirectToAction("DonorRequests", "Finder");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Username & Password is Incorrect!");
                            }
                        }
                        else if (user.UserTypeID == 3)//seeker
                        {
                            var seeker = DB.SeekerTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                            if (seeker != null)
                            {
                                Session["SeekerID"] = seeker.SeekerID;
                                Session["FullName"] = seeker.FullName;
                                Session["Age"] = seeker.Age;
                                Session["CityID"] = seeker.CityID;
                                Session["City"] = seeker.CityTable.City;
                                Session["BloodGroupID"] = seeker.BloodGroupID;
                                Session["BloodGroup"] = seeker.BloodGroupsTable.BloodGroup;
                                Session["ContactNo"] = seeker.ContactNo;
                                Session["NID"] = seeker.NID;
                                Session["GenderID"] = seeker.GenderID;
                                Session["Gender"] = seeker.GenderTable.Gender;
                                Session["RegistrationDate"] = seeker.RegistrationDate;
                                Session["Address"] = seeker.Address;
                                return RedirectToAction("ShowAllRequests", "Finder");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Username & Password is Incorrect!");
                            }
                        }
                        else if (user.UserTypeID == 4)//hospital
                        {
                            var hospital = DB.HospitalTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                            if (hospital != null)
                            {
                                Session["HospitalID"] = hospital.HospitalID;
                                Session["FullName"] = hospital.FullName;
                                Session["Address"] = hospital.Address;
                                Session["PhoneNo"] = hospital.PhoneNo;
                                Session["Website"] = hospital.Website;
                                Session["Email"] = hospital.Email;
                                Session["Location"] = hospital.Location;
                                Session["CityID"] = hospital.CityID;
                                Session["City"] = hospital.CityTable.City;
                                return RedirectToAction("ShowAllRequests", "Finder");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Username & Password is Incorrect!");
                            }
                        }
                        else if (user.UserTypeID == 5)//bloodbank
                        {
                            var bloodbank = DB.BloodBankTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                            if (bloodbank != null)
                            {
                                Session["BloodBankID"] = bloodbank.BloodBankID;
                                Session["BloodBankName"] = bloodbank.BloodBankName;
                                Session["Address"] = bloodbank.Address;
                                Session["PhoneNo"] = bloodbank.PhoneNo;
                                Session["Location"] = bloodbank.Location;
                                Session["Website"] = bloodbank.Website;
                                Session["Email"] = bloodbank.Email;
                                Session["CityID"] = bloodbank.CItyID;
                                Session["City"] = bloodbank.CityTable.City;
                                return RedirectToAction("BloodBankStock", "BloodBank");

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Username & Password is Incorrect!");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Username & Password is Incorrect!");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Username & Password is Incorrect!");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username & Password is Incorrect!");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please Provide Username & Password!");

            }
            ClearSession();
            return View(userMV);
        }
        private void ClearSession()
        {
            Session["UserID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["EmailAddress"] = string.Empty;
            Session["AccountStatusID"] = string.Empty;
            Session["AccountStatus"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["UserType"] = string.Empty;
            Session["Description"] = string.Empty;
        }
        public ActionResult Logout()
        {
            ClearSession();
            return RedirectToAction("MainHome");
        }
        public ActionResult AboutUs()
        {
            return View();
        }
    }
}
                    
